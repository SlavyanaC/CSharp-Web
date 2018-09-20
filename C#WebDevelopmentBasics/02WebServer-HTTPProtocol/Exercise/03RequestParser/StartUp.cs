namespace _03RequestParser
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text;

    class StartUp
    {
        static void Main(string[] args)
        {
            Dictionary<string, HashSet<string>> validUrsl = ProcessInput();
            string request = Console.ReadLine();

            var response = GetHttpResponse(request, validUrsl);
            Console.WriteLine(response);
        }

        private static string GetHttpResponse(string request, Dictionary<string, HashSet<string>> validUrsl)
        {
            string[] requestTokens = request.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string method = requestTokens[0].ToLower();
            string url = requestTokens[1];
            string protocol = requestTokens[2];

            bool urlAndMethodAreValid = validUrsl.ContainsKey(url) && validUrsl[url].Contains(method);
            HttpStatusCode statusCode = urlAndMethodAreValid ? HttpStatusCode.OK : HttpStatusCode.NotFound;

            StringBuilder builder = new StringBuilder()
                .AppendLine($"{protocol} {(int)statusCode} {statusCode}")
                .AppendLine($"Content-Length: {statusCode.ToString().Length}")
                .AppendLine($"Content-Type: text/plain")
                .AppendLine()
                .AppendLine($"{statusCode}");

            return builder.ToString().Trim();
        }

        private static Dictionary<string, HashSet<string>> ProcessInput()
        {
            Dictionary<string, HashSet<string>> validUrls = new Dictionary<string, HashSet<string>>();
            string input = string.Empty;
            while ((input = Console.ReadLine()) != "END")
            {
                string[] inputTokens = input.Split('/', StringSplitOptions.RemoveEmptyEntries);
                string path = inputTokens[0];
                string method = inputTokens[1].ToLower();
                string fullPath = "/" + path;

                if (!validUrls.ContainsKey(fullPath))
                {
                    validUrls[fullPath] = new HashSet<string>();
                }

                validUrls[fullPath].Add(method);
            }

            return validUrls;
        }
    }
}
