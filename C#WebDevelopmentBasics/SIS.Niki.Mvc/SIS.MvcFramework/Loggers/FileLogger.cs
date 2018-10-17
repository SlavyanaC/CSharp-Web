namespace SIS.MvcFramework.Loggers
{
    using System;
    using System.IO;
    using Contracts;

    public class FileLogger : ILogger
    {
        private static readonly object LockObject = new object();

        private readonly string fileName;

        public FileLogger(string fileName)
        {
            this.fileName = fileName;
        }

        public void Log(string message)
        {
            lock (LockObject)
            {
                File.AppendAllText(this.fileName, $"[{DateTime.UtcNow}] {message}{Environment.NewLine}");
            }
        }
    }
}
