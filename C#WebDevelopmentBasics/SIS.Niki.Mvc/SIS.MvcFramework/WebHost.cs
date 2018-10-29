﻿namespace SIS.MvcFramework
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Globalization;
    using HTTP.Enums;
    using HTTP.Requests.Contracts;
    using HTTP.Responses.Contracts;
    using WebServer.Results;
    using WebServer;
    using WebServer.Routing;
    using Attributes;
    using Services.Contracts;
    using Services;
    using Extensions;
    using Contracts;
    using Loggers.Contracts;
    using Loggers;

    public static class WebHost
    {
        public static void Start(IMvcApplication application)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

            var dependencyContainer = new ServiceCollection();

            dependencyContainer.AddService<IHashService, HashService>();
            dependencyContainer.AddService<IUserCookieService, UserCookieService>();
            dependencyContainer.AddService<ILogger>(() => new FileLogger("log.txt"));

            application.ConfigureServices(dependencyContainer);

            var serverRoutingTable = new ServerRoutingTable();
            RegisterRoutes(serverRoutingTable, application, dependencyContainer);

            application.Configure();

            var server = new Server(80, serverRoutingTable);
            server.Run();
        }

        private static void RegisterRoutes(ServerRoutingTable routingTable,
            IMvcApplication application, IServiceCollection serviceCollection)
        {
            var controllers = application.GetType().Assembly.GetTypes()
                .Where(myType => myType.IsClass
                                 && !myType.IsAbstract
                                 && myType.IsSubclassOf(typeof(Controller)));

            foreach (var controller in controllers)
            {
                var getMethods = controller.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(
                    method => method.CustomAttributes.Any(
                        ca => ca.AttributeType.IsSubclassOf(typeof(HttpAttribute))));

                foreach (var methodInfo in getMethods)
                {
                    var httpAttribute = (HttpAttribute)methodInfo.GetCustomAttributes(true)
                        .FirstOrDefault(ca =>
                            ca.GetType().IsSubclassOf(typeof(HttpAttribute)));

                    if (httpAttribute == null)
                    {
                        // TODO: Assume HttpGet
                        continue;
                    }

                    var method = httpAttribute.Method;

                    var path = httpAttribute.Path;
                    if (path == null)
                    {
                        var controllerName = controller.Name;
                        if (controllerName.EndsWith("Controller"))
                        {
                            controllerName = controllerName.Substring(0, controllerName.Length - "Controller".Length);
                        }

                        var actionName = methodInfo.Name;

                        path = $"/{controllerName.ToLower()}/{actionName.ToLower()}";
                    }
                    else if (!path.StartsWith("/"))
                    {
                        path = "/" + path;
                    }

                    routingTable.Add(method, path, (request) => ExecuteAction(controller, methodInfo, request, serviceCollection));

                    Console.WriteLine($"Route registered: {controller.Name}.{methodInfo.Name} => {method} => {path}");
                }
            }

            if (!routingTable.Routes[HttpRequestMethod.GET].ContainsKey("/") && routingTable.Routes[HttpRequestMethod.GET].ContainsKey("/home/index"))
            {
                routingTable.Routes[HttpRequestMethod.GET]["/"] = (request) =>
                    routingTable.Routes[HttpRequestMethod.GET]["/home/index"](request);

                Console.WriteLine($"Route registered: reuse /home/index => {HttpRequestMethod.GET} => /");
            }
        }

        private static IHttpResponse ExecuteAction(Type controllerType,
            MethodInfo methodInfo, IHttpRequest request,
            IServiceCollection serviceCollection)
        {
            var controllerInstance = serviceCollection.CreateInstance(controllerType) as Controller;
            if (controllerInstance == null)
            {
                return new TextResult("Controller not found.",
                    HttpResponseStatusCode.InternalServerError);
            }

            controllerInstance.Request = request;
            controllerInstance.ViewEngine = new ViewEngine.ViewEngine(); // TODO: use serviceCollection
            controllerInstance.UserCookieService = serviceCollection.CreateInstance<IUserCookieService>();

            var actionParameterObjects = GetActionParameterObjects(methodInfo, request, serviceCollection);
            var httpResponse = methodInfo.Invoke(controllerInstance, actionParameterObjects.ToArray()) as IHttpResponse;
            return httpResponse;
        }

        private static List<object> GetActionParameterObjects(MethodInfo methodInfo, IHttpRequest request,
            IServiceCollection serviceCollection)
        {
            var actionParameters = methodInfo.GetParameters();
            var actionParameterObjects = new List<object>();
            foreach (var actionParameter in actionParameters)
            {
                if (actionParameter.ParameterType.IsValueType ||
                    Type.GetTypeCode(actionParameter.ParameterType) == TypeCode.String)
                {
                    var stringValue = GetRequestData(request, actionParameter.Name);
                    actionParameterObjects.Add(ObjectMapper.TryParse(stringValue, actionParameter.ParameterType));
                }
                else
                {
                    var instance = serviceCollection.CreateInstance(actionParameter.ParameterType);
                    var properties = actionParameter.ParameterType.GetProperties();
                    foreach (var propertyInfo in properties)
                    {
                        // TODO: Support IEnumerable
                        var stringValue = GetRequestData(request, propertyInfo.Name);
                        var value = ObjectMapper.TryParse(stringValue, propertyInfo.PropertyType);

                        propertyInfo.SetMethod.Invoke(instance, new object[] { value });
                    }

                    actionParameterObjects.Add(instance);
                }
            }

            return actionParameterObjects;
        }

        private static string GetRequestData(IHttpRequest request, string key)
        {
            key = key.ToLower();
            string stringValue = null;
            if (request.FormData.Any(x => x.Key.ToLower() == key))
            {
                stringValue = request.FormData.First(x => x.Key.ToLower() == key).Value.ToString().UrlDecode();
            }
            else if (request.QueryData.Any(x => x.Key.ToLower() == key))
            {
                stringValue = request.QueryData.First(x => x.Key.ToLower() == key).Value.ToString().UrlDecode();
            }

            return stringValue;
        }
    }
}
