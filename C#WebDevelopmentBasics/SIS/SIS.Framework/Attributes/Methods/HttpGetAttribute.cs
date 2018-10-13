namespace SIS.Framework.Attributes.Methods
{
    using System;
    using HTTP.Enums;

    public class HttpGetAttribute : HttpMethodAttribute
    {
        public override bool IsValid(string requestMethod)
        {
            return string.Equals(requestMethod, HttpRequestMethod.GET.ToString(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
