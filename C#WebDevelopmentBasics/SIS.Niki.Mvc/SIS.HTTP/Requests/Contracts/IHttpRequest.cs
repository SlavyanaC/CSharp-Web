namespace SIS.HTTP.Requests.Contracts
{
    using System.Collections.Generic;
    using Enums;
    using Headers.Contracts;
    using Cookies.Contracts;
    using Sessions.Contracts;

    public interface IHttpRequest
    {
        string Path { get; }

        string Url { get; }

        Dictionary<string, object> FormData { get; }

        Dictionary<string, object> QueryData { get; }

        IHttpHeaderCollection Headers { get; }

        IHttpCookieCollection Cookies { get; }

        IHttpSession Session { get; set; }

        HttpRequestMethod RequestMethod { get; }
    }
}
