namespace SIS.HTTP.Cookies
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Contracts;

    public class HttpCookieCollection : IHttpCookieCollection
    {
        private readonly Dictionary<string, HttpCookie> cookies;

        public HttpCookieCollection()
        {
            this.cookies = new Dictionary<string, HttpCookie>();
        }

        public void Add(HttpCookie cookie)
        {
            if (cookie == null)
            {
                throw new ArgumentNullException();
            }

            this.cookies.Add(cookie.Key, cookie);
        }

        public bool ContainsCookie(string key) => this.cookies.ContainsKey(key);

        public HttpCookie GetCookie(string key) => this.cookies.FirstOrDefault(c => c.Key == key).Value;

        public bool HasCookies() => this.cookies.Any();

        public override string ToString()
        {
            return string.Join("; ", this.cookies.Values);
        }
    }
}
