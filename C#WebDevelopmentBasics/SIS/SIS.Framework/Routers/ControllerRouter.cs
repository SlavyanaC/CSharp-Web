namespace SIS.Framework.Routers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.ComponentModel.DataAnnotations;
    using HTTP.Enums;
    using WebServer.Results;
    using HTTP.Responses.Contracts;
    using HTTP.Requests.Contracts;
    using ActionResults.Contracts;
    using Attributes.Methods;
    using Controllers;
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

            // TODO: ?!?
            var actionParameters = this.MapActionParameters(controller, action, request);

            var actionResult = this.InvokeAction(controller, action, actionParameters);
            return this.PrepareResponse(actionResult);
        }

        private IActionResult InvokeAction(Controller controller, MethodInfo action, object[] actionParameters)
        {
            return (IActionResult)action.Invoke(controller, actionParameters);
        }

        private object[] MapActionParameters(Controller controller, MethodInfo action, IHttpRequest httpRequest)
        {
            var actionParametersInfo = action.GetParameters();
            var mappedActionParameters = new object[actionParametersInfo.Length];

            for (var i = 0; i < actionParametersInfo.Length; i++)
            {
                var currentParameterInfo = actionParametersInfo[i];
                if (currentParameterInfo.ParameterType.IsPrimitive ||
                    currentParameterInfo.ParameterType == typeof(string))
                {
                    mappedActionParameters[i] = this.ProcessPrimitiveParameters(currentParameterInfo, httpRequest);
                }
                else
                {
                    var bindingModel = this.ProcessBindingModelParameters(currentParameterInfo, httpRequest);
                    controller.ModelState.IsValid = this.IsValidModel(bindingModel, currentParameterInfo.ParameterType);
                    mappedActionParameters[i] = bindingModel;
                }
            }

            return mappedActionParameters;
        }

        private bool? IsValidModel(object bindingModel, Type bindingModelType)
        {
            var properties = bindingModelType.GetProperties();

            foreach (var property in properties)
            {
                var propertyValidationAttributes = property.GetCustomAttributes()
                    .Where(ca => ca is ValidationAttribute)
                    .Cast<ValidationAttribute>()
                    .ToList();

                foreach (var validationAttribute in propertyValidationAttributes)
                {
                    var propertyValue = property.GetValue(bindingModel);
                    if (!validationAttribute.IsValid(propertyValue))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private object ProcessBindingModelParameters(ParameterInfo parameter, IHttpRequest httpRequest)
        {
            var bindingModelType = parameter.ParameterType;

            var bindingModelInstance = Activator.CreateInstance(bindingModelType);
            var bindingModelProperties = bindingModelType.GetProperties();

            foreach (var property in bindingModelProperties)
            {
                try
                {
                    var value = this.GetParameterFromRequestData(httpRequest, property.Name);
                    property.SetValue(bindingModelInstance, Convert.ChangeType(value, property.PropertyType));
                }
                catch
                {
                    Console.WriteLine($"The {property.Name} field could not be mapped");
                }
            }

            return Convert.ChangeType(bindingModelInstance, bindingModelType);
        }

        private object ProcessPrimitiveParameters(ParameterInfo parameter, IHttpRequest httpRequest)
        {
            var value = this.GetParameterFromRequestData(httpRequest, parameter.Name);
            return Convert.ChangeType(value, parameter.ParameterType);
        }

        private object GetParameterFromRequestData(IHttpRequest httpRequest, string parameterName)
        {
            if (httpRequest.QueryData.ContainsKey(parameterName))
            {
                return httpRequest.QueryData[parameterName];
            }
            if (httpRequest.FormData.ContainsKey(parameterName))
            {
                return httpRequest.FormData[parameterName];
            }

            return null;
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

        private IHttpResponse PrepareResponse(IActionResult actionResult)
        {
            var invocationType = actionResult.Invoke();

            switch (actionResult)
            {
                case IViewable _:
                    return new HtmlResult(invocationType, HttpResponseStatusCode.Ok);
                case IRedirectable _:
                    return new RedirectResult(invocationType);
                default:
                    throw new InvalidOperationException("Unsupported action!");
            }
        }
    }
}
