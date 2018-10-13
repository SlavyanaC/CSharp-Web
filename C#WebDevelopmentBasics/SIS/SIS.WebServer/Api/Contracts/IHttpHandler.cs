﻿namespace SIS.WebServer.Api.Contracts
{
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;

    public interface IHttpHandler
    {
        IHttpResponse Handle(IHttpRequest request);
    }
}
