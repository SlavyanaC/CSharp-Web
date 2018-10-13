namespace SIS.WebServer.Api
{
    using System.Collections;
    using System.IO;
    using HTTP.Common;
    using HTTP.Enums;
    using SIS.HTTP.Requests.Contracts;
    using HTTP.Responses;
    using SIS.HTTP.Responses.Contracts;
    using Contracts;
    using Results;
    using Routing;

    public class HttpHandler : IHttpHandler
    {
        private const string RootDirectoryRelativePath = "../../..";
        private readonly ServerRoutingTable serverRoutingTable;

        public HttpHandler(ServerRoutingTable routingTable)
        {
            this.serverRoutingTable = routingTable;
        }

        public IHttpResponse Handle(IHttpRequest httpRequest)
        {
            var isResourceRequest = this.IsResourceRequest(httpRequest);
            if (isResourceRequest)
            {
                return this.HandleRequestResponse(httpRequest.Path);
            }

            //TODO: Add ...Path.ToLower()
            if (!this.serverRoutingTable.Routes.ContainsKey(httpRequest.RequestMethod) || !this.serverRoutingTable.Routes[httpRequest.RequestMethod].ContainsKey(httpRequest.Path))
            {
                return new HttpResponse(HttpResponseStatusCode.NotFound);
            }

            return this.serverRoutingTable.Routes[httpRequest.RequestMethod][httpRequest.Path].Invoke(httpRequest);
        }

        private bool IsResourceRequest(IHttpRequest httpRequest)
        {
            var requestPath = httpRequest.Path;
            if (!requestPath.Contains('.'))
            {
                return false;
            }

            var requestPathExtension = requestPath
                .Substring(requestPath.LastIndexOf('.'));
            return ((IList)GlobalConstants.ResourceExtensions).Contains(requestPathExtension);
        }

        private IHttpResponse HandleRequestResponse(string httpRequestPath)
        {
            var extensionStartIndex = httpRequestPath.LastIndexOf('.');
            var resourceNameStartIndex = httpRequestPath.LastIndexOf('/');

            var requestExtension = httpRequestPath.Substring(extensionStartIndex);

            var resourceName = httpRequestPath.Substring(resourceNameStartIndex);

            var resourcePath = RootDirectoryRelativePath +
                               "/Resources" +
                               $"/{requestExtension.Substring(1)}" +
                               resourceName;

            if (!File.Exists(resourcePath))
            {
                return new HttpResponse(HttpResponseStatusCode.NotFound);
            }

            var fileContent = File.ReadAllBytes(resourcePath);

            return new InlineResourceResult(fileContent, HttpResponseStatusCode.Ok);
        }
    }
}
