namespace SIS.HTTP.Cookies
{
    using System;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;
    using Common;
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
                CoreValidator.ThrowIfNull(cookie, nameof(cookie));
                throw new ArgumentNullException();
            }

            if (this.ContainsCookie(cookie.Key))
            {
                return;
            }

            CoreValidator.ThrowIfNull(cookie, nameof(cookie));
            this.cookies.Add(cookie.Key, cookie);
        }

        public bool ContainsCookie(string key)
        {
            CoreValidator.ThrowIfNull(cookies, nameof(key));
            return this.cookies.ContainsKey(key);
        }

        public HttpCookie GetCookie(string key)
        {
            CoreValidator.ThrowIfNull(key, nameof(key));
            return this.cookies.FirstOrDefault(c => c.Key == key).Value;
        }

        public bool HasCookies() => this.cookies.Any();

        public IEnumerator<HttpCookie> GetEnumerator()
        {
            foreach (var cookie in this.cookies)
            {
                yield return cookie.Value;
            }
        }

        public bool CookieIsNew(string key, string value)
        {
            return this.ContainsCookie(key) && value != null && this.cookies[key].Value == value;
        }

        public override string ToString()
        {
            return string.Join("; ", this.cookies.Values);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
