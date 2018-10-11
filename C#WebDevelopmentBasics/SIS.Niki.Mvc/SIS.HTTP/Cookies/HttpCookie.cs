namespace SIS.HTTP.Cookies
{
    using System;
    using Common;

    public class HttpCookie
    {
        private const int HttpCookieDefaultExpirationInDays = 3;

        public HttpCookie(string key, string value, int expirationInDays = HttpCookieDefaultExpirationInDays)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            CoreValidator.ThrowIfNullOrEmpty(value, nameof(value));

            this.Key = key;
            this.Value = value;
            this.IsNew = true;
            this.Path = "/";
            this.IsHttpOnly = true;
            this.Expires = DateTime.UtcNow.AddDays(expirationInDays);
        }

        public HttpCookie(string key, string value, bool isNew, int expirationInDays = HttpCookieDefaultExpirationInDays)
            : this(key, value, expirationInDays)
        {
            this.IsNew = isNew;
        }

        public string Key { get; }

        public string Value { get; }

        public DateTime Expires { get; private set; }

        public bool IsNew { get; }

        public string Path { get; private set; }

        public bool IsHttpOnly { get; set; }

        public void Delete()
        {
            this.Expires = DateTime.UtcNow.AddDays(-1);
        }

        public void SetPath(string path)
        {
            this.Path = path;
        }

        public override string ToString()
        {
            var result = $"{this.Key}={this.Value}; Expires={this.Expires:R}; Path={this.Path}";
            if (this.IsHttpOnly)
            {
                result += "; HttpOnly";
            }

            return result;
        }
    }
}
