namespace SIS.WebServer
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;
    using HTTP.Requests;
    using HTTP.Requests.Contracts;
    using HTTP.Responses.Contracts;
    using HTTP.Enums;
    using HTTP.Responses;
    using HTTP.Cookies;
    using HTTP.Common;
    using HTTP.Sessions;
    using HTTP.Exceptions;
    using Routing;
    using Results;
    using Api.Contracts;

    public class ConnectionHandler
    {
        //private const string RESOURCES_DIRECTORY_RELATIVE_PATH = "../../../Resources/";

        //private readonly string[] resoureExtentions = { "css", "js" };
        private readonly Socket client;
        //private readonly ServerRoutingTable serverRoutingTable;
        private readonly IHttpHandler handler;

        //public ConnectionHandler(Socket client, ServerRoutingTable serverRoutingTable)
        //{
        //    CoreValidator.ThrowIfNull(client, nameof(client));
        //    CoreValidator.ThrowIfNull(serverRoutingTable, nameof(serverRoutingTable));
        //    this.client = client;
        //    this.serverRoutingTable = serverRoutingTable;
        //}

        public ConnectionHandler(Socket client, IHttpHandler handler)
        {
            CoreValidator.ThrowIfNull(client, nameof(client));
            CoreValidator.ThrowIfNull(handler, nameof(handler));

            this.client = client;
            this.handler = handler;
        }

        public async Task ProcessRequestAsync()
        {
            try
            {
                var httpRequest = await this.ReadRequest();

                if (httpRequest != null)
                {
                    var sessionId = this.SetRequestSession(httpRequest);
                    var httpResponse = this.handler.Handle(httpRequest);

                    this.SetResponseSession(httpResponse, sessionId);
                    await this.PrepareResponse(httpResponse);
                }
            }
            catch (BadRequestException e)
            {
                await this.PrepareResponse(new TextResult(e.ToString(), HttpResponseStatusCode.BadRequest));
            }
            catch (Exception e)
            {
                await this.PrepareResponse(new TextResult(e.ToString(), HttpResponseStatusCode.InternalServerError));
            }

            this.client.Shutdown(SocketShutdown.Both);
        }

        private async Task<IHttpRequest> ReadRequest()
        {
            var result = new StringBuilder();
            var data = new ArraySegment<byte>(new byte[1024]);

            while (true)
            {
                var numberOfBytesRead = await this.client.ReceiveAsync(data.Array, SocketFlags.None);

                if (numberOfBytesRead == 0)
                {
                    break;
                }

                var bytesAsString = Encoding.UTF8.GetString(data.Array, 0, numberOfBytesRead);
                result.Append(bytesAsString);

                if (numberOfBytesRead < 1024)
                {
                    break;
                }
            }

            if (result.Length == 0)
            {
                return null;
            }

            return new HttpRequest(result.ToString());
        }

        //private IHttpResponse HandleRequest(IHttpRequest httpRequest)
        //{
        //    if (!this.serverRoutingTable.Routes.ContainsKey(httpRequest.RequestMethod) || !this.serverRoutingTable.Routes[httpRequest.RequestMethod].ContainsKey(httpRequest.Path))
        //    {
        //        return this.ReturnIfResource(httpRequest.Path);
        //    }

        //    return this.serverRoutingTable.Routes[httpRequest.RequestMethod][httpRequest.Path].Invoke(httpRequest);
        //}

        //private IHttpResponse ReturnIfResource(string httpRequestPath)
        //{
        //    var fileExtension = httpRequestPath.Substring(httpRequestPath.LastIndexOf('.') + 1);
        //    var resourcePath = httpRequestPath.Substring(httpRequestPath.LastIndexOf('/'));

        //    if (!resoureExtentions.Contains(fileExtension))
        //    {
        //        return new HttpResponse(HttpResponseStatusCode.NotFound);
        //    }

        //    var pathToSearch = RESOURCES_DIRECTORY_RELATIVE_PATH +
        //                       fileExtension +
        //                       resourcePath;

        //    if (!File.Exists(pathToSearch))
        //    {
        //        return new HttpResponse(HttpResponseStatusCode.NotFound);
        //    }

        //    var fileBytes = File.ReadAllBytes(pathToSearch);
        //    return new InlineResourceResult(fileBytes, HttpResponseStatusCode.Ok);
        //}

        private async Task PrepareResponse(IHttpResponse httpResponse)
        {
            var byteSegments = httpResponse.GetBytes();
            await this.client.SendAsync(byteSegments, SocketFlags.None);
        }

        private string SetRequestSession(IHttpRequest httpRequest)
        {
            string sessionId = null;

            if (httpRequest.Cookies.ContainsCookie(HttpSessionStorage.SessionCookieKey))
            {
                var cookie = httpRequest.Cookies.GetCookie(HttpSessionStorage.SessionCookieKey);
                sessionId = cookie.Value;
                httpRequest.Session = HttpSessionStorage.GetSession(sessionId);
            }
            else
            {
                sessionId = Guid.NewGuid().ToString();
                httpRequest.Session = HttpSessionStorage.GetSession(sessionId);
            }

            return sessionId;
        }

        private void SetResponseSession(IHttpResponse httpResponse, string sessionId)
        {
            if (sessionId != null)
            {
                httpResponse.AddCookie(new HttpCookie(HttpSessionStorage.SessionCookieKey, sessionId));
            }
        }
    }
}
