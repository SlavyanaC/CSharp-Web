namespace SIS.HTTP.Headers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Common;

    public class HttpHeaderCollection : IHttpHeaderCollection
    {
        private readonly Dictionary<string, HttpHeader> headers;

        public HttpHeaderCollection()
        {
            this.headers = new Dictionary<string, HttpHeader>();
        }

        public void Add(HttpHeader header)
        {
            CoreValidator.ThrowIfNull(header, nameof(header));
            this.headers[header.Key] = header;
        }

        public bool ContainsHeader(string key)
        {
            CoreValidator.ThrowIfNull(key, nameof(key));
            return this.headers.ContainsKey(key);
        }

        public HttpHeader GetHeader(string key)
        {
            CoreValidator.ThrowIfNull(key, nameof(key));
            return this.headers.FirstOrDefault(h => h.Key == key).Value;
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, headers.Values);
        }
    }
}
