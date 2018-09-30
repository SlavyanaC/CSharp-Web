namespace SIS.HTTP.Requests.Contrcts
{
    using System.Collections.Generic;
    using Enums;
    using Headers.Contracts;

    public interface IHttpRequest
    {
        string Paht { get; }

        string Url { get; }

        Dictionary<string, object> FormData { get; }

        Dictionary<string, object> QueryData { get; }

        IHttpHeaderCollection Headers { get; }

        HttpRequestMethod RequestMethod { get; }
    }
}
