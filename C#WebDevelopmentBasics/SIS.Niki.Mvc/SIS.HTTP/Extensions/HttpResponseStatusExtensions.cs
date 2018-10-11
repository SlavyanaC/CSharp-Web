namespace SIS.HTTP.Extensions
{
    using System;
    using Enums;
    using Exceptions;

    public static class HttpResponseStatusExtensions
    {
        public static string GetResponseLine(this HttpResponseStatusCode statusCode)
        {
            bool isValid = Enum.TryParse(statusCode.ToString(), out HttpResponseStatusCode responseStatusCode);

            if (!isValid)
            {
                throw new BadRequestException();
            }

            return $"{(int)responseStatusCode} {responseStatusCode}";
        }
    }
}
