namespace SIS.Framework.Attributes.Methods
{
    using System;
    using SIS.HTTP.Enums;

    public class HttpDeleteAttribute : HttpMethodAttribute
    {
        public override bool IsValid(string requestMethod)
        {
            return string.Equals(requestMethod, HttpRequestMethod.DELETE.ToString(),
                StringComparison.OrdinalIgnoreCase);
        }
    }
}
