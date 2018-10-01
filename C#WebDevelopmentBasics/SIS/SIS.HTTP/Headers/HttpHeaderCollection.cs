namespace SIS.HTTP.Headers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;

    public class HttpHeaderCollection : IHttpHeaderCollection
    {
        private readonly Dictionary<string, HttpHeader> headers;

        public HttpHeaderCollection()
        {
            this.headers = new Dictionary<string, HttpHeader>();
        }

        public int MyProperty { get; set; }

        public void Add(HttpHeader header) => this.headers[header.Key] = header;

        public bool ContainsHeader(string key) => this.headers.ContainsKey(key);

        public HttpHeader GetHeader(string key) => this.headers.FirstOrDefault(h => h.Key == key).Value;

        public override string ToString()
        {
            return string.Join(Environment.NewLine, headers.Values);
        }
    }
}
