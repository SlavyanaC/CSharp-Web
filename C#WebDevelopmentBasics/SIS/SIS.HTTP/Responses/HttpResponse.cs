using System;

namespace SIS.HTTP.Responses
{
    using System.Linq;
    using System.Text;
    using Common;
    using Enums;
    using Extensions;
    using Headers;
    using Headers.Contracts;
    using Contracts;
    using Cookies;
    using Cookies.Contracts;

    public class HttpResponse : IHttpResponse
    {
        public HttpResponse() { }

        public HttpResponse(HttpResponseStatusCode statusCode)
        {
            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookieCollection();
            this.Content = new byte[0];
            this.StatusCode = statusCode;
        }

        public HttpResponseStatusCode StatusCode { get; set; }

        public IHttpHeaderCollection Headers { get; private set; }

        public IHttpCookieCollection Cookies { get; }

        public byte[] Content { get; set; }

        public void AddHeader(HttpHeader header) => this.Headers.Add(header);

        public void AddCookie(HttpCookie cookie) => this.Cookies.Add(cookie);

        public byte[] GetBytes()
        {
            return Encoding.UTF8.GetBytes(this.ToString()).Concat(this.Content).ToArray();
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.Append($"{GlobalConstants.HttpOneProtocolFragment} {this.StatusCode.GetResponseLine()}")
                .Append(Environment.NewLine)
                .Append(this.Headers).Append(Environment.NewLine);

            if (this.Cookies.HasCookies())
            {
                result.Append($"Set-Cookie: {this.Cookies}")
                    .Append(Environment.NewLine);
            }

            result.Append(Environment.NewLine);

            return result.ToString();
        }
    }
}
