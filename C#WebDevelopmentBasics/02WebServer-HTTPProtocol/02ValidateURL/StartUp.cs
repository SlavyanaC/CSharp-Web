namespace _02ValidateURL
{
    using System;
    using System.Net;
    using System.Text;

    class StartUp
    {
        static void Main(string[] args)
        {
            var inputUrl = string.Empty;
            while ((inputUrl = Console.ReadLine()) != "exit")
            {
                var decodedUrl = WebUtility.UrlDecode(inputUrl);
                var parsedUrl = new Uri(decodedUrl);

                if (!IsGiven(parsedUrl.Scheme) || !IsGiven(parsedUrl.Host) || !parsedUrl.IsDefaultPort || !IsGiven(parsedUrl.LocalPath))
                {
                    Console.WriteLine("Invalid URL");
                    continue;
                }

                var builder = new StringBuilder();
                builder.AppendLine($"Protocol: {parsedUrl.Scheme}")
                       .AppendLine($"Host: {parsedUrl.Host}")
                       .AppendLine($"Port: {parsedUrl.Port}")
                       .AppendLine($"Path: {parsedUrl.LocalPath}");

                if (IsGiven(parsedUrl.Query))
                    builder.AppendLine($"Query: {parsedUrl.Query.TrimStart('?')}");

                if (IsGiven(parsedUrl.Fragment))
                    builder.AppendLine($"Fragment: {parsedUrl.Fragment.TrimStart('#')}");

                Console.WriteLine(builder.ToString().TrimEnd());
            }
        }

        private static bool IsGiven(string property)
        {
            return string.IsNullOrWhiteSpace(property) == true ? false : true;
        }
    }
}
