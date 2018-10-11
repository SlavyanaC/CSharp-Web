namespace SIS.MvcFramework.Attributes
{
    using HTTP.Enums;

    public class HttpGetAttribute : HttpAttribute
    {
        public HttpGetAttribute(string path)
            : base(path) { }

        public override HttpRequestMethod Method => HttpRequestMethod.GET;
    }
}
