namespace SIS.MvcFramework.Attributes
{
    using System;
    using HTTP.Enums;

    [AttributeUsage(AttributeTargets.Method)]
    public abstract class HttpAttribute : Attribute
    {
        protected HttpAttribute(string path)
        {
            this.Path = path;
        }

        public string Path { get; }

        public abstract HttpRequestMethod Method { get; }
    }
}
