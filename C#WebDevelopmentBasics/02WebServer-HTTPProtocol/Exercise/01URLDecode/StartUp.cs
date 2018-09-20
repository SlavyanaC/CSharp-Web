namespace _01URLDecode
{
    using System;
    using System.Net;

    class StartUp
    {
        static void Main(string[] args)
        {
            var inputUrl = Console.ReadLine();
            var decodedInput = WebUtility.UrlDecode(inputUrl);
            Console.WriteLine(decodedInput);
        }
    }
}
