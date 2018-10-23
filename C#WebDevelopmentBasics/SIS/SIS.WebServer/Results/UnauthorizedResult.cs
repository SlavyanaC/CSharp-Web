namespace SIS.WebServer.Results
{
    using System.Text;
    using HTTP.Enums;
    using HTTP.Headers;
    using HTTP.Responses;

    public class UnauthorizedResult : HttpResponse
    {
        private const string DefaultErrorHeading = "<h1>Yuo have no permission to access this functionality.</h1>";

        public UnauthorizedResult()
            : base(HttpResponseStatusCode.Unauthorized)
        {
            this.Headers.Add(new HttpHeader(HttpHeader.ContentType, "text/html"));
            this.Content = Encoding.UTF8.GetBytes(DefaultErrorHeading);
        }
    }
}
