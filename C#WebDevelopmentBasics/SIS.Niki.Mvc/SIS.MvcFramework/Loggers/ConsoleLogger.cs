namespace SIS.MvcFramework.Loggers
{
    using System;
    using Contracts;

    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"[{DateTime.UtcNow}] {message}");
        }
    }
}
