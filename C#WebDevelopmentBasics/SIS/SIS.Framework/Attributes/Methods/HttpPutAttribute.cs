namespace SIS.Framework.Attributes.Methods
{
    using System;
    using HTTP.Enums;

    public class HttpPutAttribute : HttpMethodAttribute
    {
        public override bool IsValid(string requestMethod)
        {
            return string.Equals(requestMethod, HttpRequestMethod.PUT.ToString(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
