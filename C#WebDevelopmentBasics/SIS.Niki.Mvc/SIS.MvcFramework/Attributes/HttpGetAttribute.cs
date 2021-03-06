﻿namespace SIS.MvcFramework.Attributes
{
    using HTTP.Enums;

    public class HttpGetAttribute : HttpAttribute
    {
        public HttpGetAttribute(string path = null)
            : base(path) { }

        public override HttpRequestMethod Method => HttpRequestMethod.GET;
    }
}
