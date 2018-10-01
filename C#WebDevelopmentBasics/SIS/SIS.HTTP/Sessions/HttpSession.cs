namespace SIS.HTTP.Sessions
{
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;

    public class HttpSession : IHttpSession
    {
        private readonly Dictionary<string, object> sessionParameters;

        public HttpSession(string id)
        {
            this.Id = id;
            this.sessionParameters = new Dictionary<string, object>();
        }

        public string Id { get; }

        public object GetParameter(string name) => this.sessionParameters.FirstOrDefault(p => p.Key == name).Value;

        public bool ContainsParameter(string name) => this.sessionParameters.ContainsKey(name);

        public void AddParameter(string name, object parameter) => this.sessionParameters.Add(name, parameter);

        public void ClearParameters() => this.sessionParameters.Clear();
    }
}
