namespace SIS.MvcFramework.Extensions
{
    using System.Net;

    public static class StringExtension
    {
        public static string UrlDecode(this string input)
        {
            return WebUtility.UrlDecode(input);
        }

        public static decimal ToDecimal(this string input)
        {
            return decimal.TryParse(input, out var value) ? value : default(decimal);
        }
    }
}
