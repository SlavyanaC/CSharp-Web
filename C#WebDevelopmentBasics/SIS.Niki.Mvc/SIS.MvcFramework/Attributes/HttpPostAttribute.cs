﻿namespace SIS.MvcFramework.Attributes
{
    using HTTP.Enums;

    public class HttpPostAttribute : HttpAttribute
    {
        public HttpPostAttribute(string path = null)
            : base(path) { }

        public override HttpRequestMethod Method => HttpRequestMethod.POST;
    }
}
