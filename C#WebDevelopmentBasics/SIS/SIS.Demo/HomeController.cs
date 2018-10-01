namespace SIS.Demo
{
    using HTTP.Responses.Contracts;
    using HTTP.Enums;
    using WebServer.Results;

    public class HomeController
    {
        public IHttpResponse Index()
        {
            string content = "<h1>Hello, World!</h1>";
            return new HtmlResult(content, HttpResponseStatusCode.Ok);
        }
    }
}
