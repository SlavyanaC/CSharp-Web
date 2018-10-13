namespace SIS.Framework.Attributes.Methods
{
    using System;
    using HTTP.Enums;

    public class HttpPostAttribute : HttpMethodAttribute
    {
        public override bool IsValid(string requestMethod)
        {
            return string.Equals(requestMethod, HttpRequestMethod.POST.ToString(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
