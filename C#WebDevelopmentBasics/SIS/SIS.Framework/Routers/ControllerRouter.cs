using SIS.HTTP.Enums;
using SIS.WebServer.Results;

namespace SIS.Framework.Routers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using ActionResults.Contracts;
    using Attributes.Methods;
    using Controllers;
    using HTTP.Requests.Contracts;
    using HTTP.Responses.Contracts;
    using WebServer.Api.Contracts;

    public class ControllerRouter : IHttpHandler
    {
        public IHttpResponse Handle(IHttpRequest request)
        {
            var controllerName = string.Empty;
            var actionName = string.Empty;
            var requetMethod = request.RequestMethod.ToString();

            if (request.Url == "/")
            {
                controllerName = "Home";
                actionName = "Index";
            }
            else
            {
                var requestUrlSplit = request.Url.Split("/", StringSplitOptions.RemoveEmptyEntries);

                controllerName = requestUrlSplit[0];
                actionName = requestUrlSplit[1];
            }

            var controller = this.GetController(controllerName, request);
            var action = this.GetMethod(requetMethod, controller, actionName);

            if (controller == null || action == null)
            {
                throw new NullReferenceException();
            }

            return this.PrepareResponse(controller, action);
        }

        private Controller GetController(string controllerName, IHttpRequest request)
        {
            if (string.IsNullOrWhiteSpace(controllerName))
            {
                return null;
            }

            var controllerTypeName = string.Format("{0}.{1}.{2}{3}, {0}",
                MvcContext.Get.AssemblyName,
                MvcContext.Get.ControllersFolder,
                controllerName,
                "Controller");

            var controllerType = Type.GetType(controllerTypeName);
            var controller = (Controller)Activator.CreateInstance(controllerType);

            if (controller != null)
            {
                controller.Request = request;
            }

            return controller;
        }

        private MethodInfo GetMethod(string requestMethod, Controller controller, string actionName)
        {
            MethodInfo method = null;

            foreach (var methodInfo in this.GetSuitableMethods(controller, actionName))
            {
                var attributes = methodInfo.GetCustomAttributes()
                    .Where(attr => attr is HttpMethodAttribute)
                    .Cast<HttpMethodAttribute>();

                if (!attributes.Any() && requestMethod.ToUpper() == "GET")
                {
                    return methodInfo;
                }

                foreach (var attribute in attributes)
                {
                    if (attribute.IsValid(requestMethod))
                    {
                        return methodInfo;
                    }
                }
            }

            return method;
        }

        private IEnumerable<MethodInfo> GetSuitableMethods(Controller controller, string actionName)
        {
            if (controller == null)
            {
                return new MethodInfo[0];
            }

            return controller.GetType()
                .GetMethods()
                .Where(mi => string.Equals(mi.Name, actionName, StringComparison.OrdinalIgnoreCase));
        }

        //TODO: Check this
        private IHttpResponse PrepareResponse(Controller controller, MethodInfo action)
        {
            var actionResult = (IActionResult)action.Invoke(controller, null);
            var invocationType = actionResult.Invoke();

            if (actionResult is IViewable)
            {
                return new HtmlResult(invocationType, HttpResponseStatusCode.Ok);
            }
            else if (actionResult is IRedirectable)
            {
                return new RedirectResult(invocationType);
            }

            return null;
        }
    }
}
