namespace SIS.HTTP.Extensions
{
    public static class StringExtensions
    {
        // TODO: Make this smarter!
        public static string Capitalize(string input) => input[0].ToString().ToUpper() + input.Substring(1);
    }
}
