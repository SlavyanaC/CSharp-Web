namespace SIS.HTTP.Requests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Enums;
    using Headers.Contracts;
    using Headers;
    using Contracts;
    using Exceptions;
    using Cookies;
    using Cookies.Contracts;
    using Sessions.Contracts;

    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestString)
        {
            this.FormData = new Dictionary<string, object>();
            this.QueryData = new Dictionary<string, object>();
            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookieCollection();

            this.ParseRequest(requestString);
        }

        public string Path { get; private set; }

        public string Url { get; private set; }

        public Dictionary<string, object> FormData { get; }

        public Dictionary<string, object> QueryData { get; }

        public IHttpHeaderCollection Headers { get; }

        public IHttpCookieCollection Cookies { get; }

        public IHttpSession Session { get; set; }

        public HttpRequestMethod RequestMethod { get; private set; }

        private void ParseRequest(string requestString)
        {
            string[] splitRequestContent = requestString.Split(Environment.NewLine, StringSplitOptions.None);
            string[] requestLine = splitRequestContent[0].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (!this.IsValidRequestLine(requestLine))
            {
                throw new BadRequestException();
            }

            this.ParseRequestMethod(requestLine);
            this.ParseRequestUrl(requestLine);
            this.ParseRequestPath();

            this.ParseHeaders(splitRequestContent.Skip(1).ToArray());
            this.ParseCookies();
            this.ParseRequestParameters(splitRequestContent.Last());
        }

        private bool IsValidRequestLine(string[] requestLine) => requestLine.Length == 3 && requestLine[2].ToUpper() == "HTTP/1.1";

        private void ParseRequestMethod(string[] requestLine)
        {
            bool methodIsValid = Enum.TryParse(requestLine[0], out HttpRequestMethod requestMethod);
            if (!methodIsValid)
            {
                throw new BadRequestException();
            }

            this.RequestMethod = requestMethod;
        }

        private void ParseRequestUrl(string[] requestLine) => this.Url = requestLine[1];

        private void ParseRequestPath()
        {
            string path = this.Url.Split('?', '#', StringSplitOptions.RemoveEmptyEntries)[0];
            this.Path = path;
        }

        private void ParseHeaders(string[] requestContent)
        {
            int endIndes = Array.IndexOf(requestContent, string.Empty);
            for (int i = 0; i < endIndes; i++)
            {
                string[] headerArgs = requestContent[i].Split(": ", StringSplitOptions.RemoveEmptyEntries);

                if (headerArgs.Length == 2)
                {
                    string key = headerArgs[0];
                    string value = headerArgs[1];

                    HttpHeader header = new HttpHeader(key, value);
                    this.Headers.Add(header);
                }

                if (!this.Headers.ContainsHeader("Host"))
                {
                    throw new BadRequestException();
                }
            }
        }

        private void ParseCookies()
        {
            if (!this.Headers.ContainsHeader("Cookie"))
            {
                return;
            }

            string cookiesString = this.Headers.GetHeader("Cookie").Value;
            if (string.IsNullOrWhiteSpace(cookiesString))
            {
                return;
            }

            string[] splitCookies = cookiesString.Split("; ");
            foreach (var splitCookie in splitCookies)
            {
                string[] cookieParts = splitCookie.Split('=', 2);
                if (cookieParts.Length != 2)
                {
                    continue; ;
                }

                string key = cookieParts[0];
                string value = cookieParts[1];

                HttpCookie cookie = new HttpCookie(key, value, false);
                this.Cookies.Add(cookie);
            }
        }

        private void ParseRequestParameters(string formData)
        {
            this.ParseQueryParameters();
            this.ParseFormDataParameters(formData);
        }

        private void ParseQueryParameters()
        {
            if (!this.Url.Contains('?'))
            {
                return;
            }

            string queryString = this.Url.Split('?')[1];
            string[] queryParameters = queryString.Split('&');

            if (!IsValidRequestQueryString(queryString, queryParameters))
            {
                throw new BadRequestException();
            }

            foreach (var query in queryParameters)
            {
                string[] queryData = query.Split('=');
                string queryKey = queryData[0];
                object queryValue = queryData[1];

                this.QueryData[queryKey] = queryValue;
            }
        }

        private bool IsValidRequestQueryString(string queryString, string[] queryParameters) => !string.IsNullOrWhiteSpace(queryString) && queryParameters.Length > 0;

        private void ParseFormDataParameters(string formData)
        {
            if (this.RequestMethod != HttpRequestMethod.POST)
            {
                return;
            }

            string[] postRequests = formData.Split('&');
            foreach (var postRequest in postRequests)
            {
                string[] postRequestData = postRequest.Split('=');
                string postRequestKey = postRequestData[0];
                object postRequestValue = postRequestData[1];

                this.FormData[postRequestKey] = postRequestValue;
            }
        }
    }
}
