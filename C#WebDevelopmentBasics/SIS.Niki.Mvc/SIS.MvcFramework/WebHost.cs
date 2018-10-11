namespace SIS.MvcFramework
{
    using System;
    using System.Linq;
    using System.Reflection;
    using HTTP.Enums;
    using HTTP.Requests.Contracts;
    using HTTP.Responses.Contracts;
    using Attributes;
    using WebServer.Results;
    using WebServer;
    using WebServer.Routing;

    public static class WebHost
    {
        public static void Start(IMvcApplication application)
        {
            application.ConfigureService();
            var serverRoutingTable = new ServerRoutingTable();

            AutoRegisterRoutes(serverRoutingTable, application);
            application.Configure();

            var server = new Server(80, serverRoutingTable);
            server.Run();
        }

        private static void AutoRegisterRoutes(ServerRoutingTable routingTable, IMvcApplication application)
        {
            var controllers = application.GetType().Assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Controller)));

            foreach (var controller in controllers)
            {
                var getMethods = controller.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Where(m => m.CustomAttributes
                        .Any(ca => ca.AttributeType.IsSubclassOf(typeof(HttpAttribute))));

                foreach (var methodInfo in getMethods)
                {
                    var httpAttribute = (HttpAttribute)methodInfo.GetCustomAttributes(true)
                        .FirstOrDefault(ca => ca.GetType().IsSubclassOf(typeof(HttpAttribute)));

                    if (httpAttribute == null)
                    {
                        continue;
                    }

                    routingTable
                        .Add(httpAttribute.Method, httpAttribute.Path, (request) => ExecuteAction(controller, methodInfo, request));

                    Console.WriteLine($"Route registered: {controller.Name}.{methodInfo.Name} => {httpAttribute.Method} => {httpAttribute.Path}");
                }
            }
        }

        private static IHttpResponse ExecuteAction(Type controllerType, MethodInfo methodInfo, IHttpRequest request)
        {
            var controllerInstance = (Controller)Activator.CreateInstance(controllerType);
            if (controllerInstance == null)
            {
                return new TextResult("Controller not found.", HttpResponseStatusCode.InternalServerError);
            }

            controllerInstance.Request = request;

            var httpResponse = (IHttpResponse)methodInfo.Invoke(controllerInstance, new object[] { });
            return httpResponse;
        }
    }
}
