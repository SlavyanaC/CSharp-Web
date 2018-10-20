namespace SIS.Framework.Routers
{
    using System.IO;
    using System.Linq;
    using HTTP.Enums;
    using HTTP.Requests.Contracts;
    using HTTP.Responses;
    using HTTP.Responses.Contracts;
    using WebServer.Api.Contracts;
    using WebServer.Results;

    public class ResourceRouter : IHttpHandler
    {
        private const string RESOURCES_DIRECTORY_RELATIVE_PATH = "../../../Resources/";
        private readonly string[] resoureExtentions = { "css", "js" };

        public IHttpResponse Handle(IHttpRequest request)
        {
            var httpRequestPath = request.Path;

            var fileExtension = httpRequestPath.Substring(httpRequestPath.LastIndexOf('.') + 1);
            var resourcePath = httpRequestPath.Substring(httpRequestPath.LastIndexOf('/'));

            if (!resoureExtentions.Contains(fileExtension))
            {
                return new HttpResponse(HttpResponseStatusCode.NotFound);
            }

            var pathToSearch = RESOURCES_DIRECTORY_RELATIVE_PATH +
                               fileExtension +
                               resourcePath;

            if (!File.Exists(pathToSearch))
            {
                return new HttpResponse(HttpResponseStatusCode.NotFound);
            }

            var fileBytes = File.ReadAllBytes(pathToSearch);
            return new InlineResourceResult(fileBytes, HttpResponseStatusCode.Ok);
        }
    }
}
