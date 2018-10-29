namespace SIS.MvcFramework
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using SIO = System.IO;
    using HTTP.Extensions;
    using HTTP.Cookies.Contracts;
    using HTTP.Headers;
    using HTTP.Responses;
    using HTTP.Enums;
    using HTTP.Requests.Contracts;
    using HTTP.Responses.Contracts;
    using ViewEngine.Contracts;
    using ViewEngine;
    using Services.Contracts;

    public abstract class Controller
    {
        protected Controller()
        {
            this.Response = new HttpResponse { StatusCode = HttpResponseStatusCode.Ok };
        }

        public IHttpRequest Request { get; set; }

        public IHttpResponse Response { get; set; }

        public IViewEngine ViewEngine { get; set; }

        public IUserCookieService UserCookieService { get; internal set; }

        protected string User => GetUserData(this.Request.Cookies, this.UserCookieService);

        public static string GetUserData(IHttpCookieCollection cookieCollection,
            IUserCookieService userCookieService)
        {
            if (!cookieCollection.ContainsCookie(".auth"))
            {
                return null;
            }

            var cookie = cookieCollection.GetCookie(".auth");
            var cookieContent = cookie.Value;
            var userName = userCookieService.GetUserData(cookieContent);
            return userName;
        }

        protected IHttpResponse View(string viewName = null, string layoutName = "_Layout")
        {
            return this.View(viewName, (object)null, layoutName);
        }

        protected IHttpResponse View<T>(T model = null, string layoutName = "_Layout")
            where T : class
        {
            return this.View(null, model, layoutName);
        }

        protected IHttpResponse View<T>(string viewName = null, T model = null, string layoutName = "_Layout")
            where T : class
        {
            if (viewName == null)
            {
                viewName = this.Request.Path.Trim('/', '\\');
                if (string.IsNullOrWhiteSpace(viewName))
                {
                    viewName = "Home/Index";
                }
            }

            var allContent = this.GetViewContent(viewName, model, layoutName);
            this.PrepareHtmlResult(allContent);
            return this.Response;
        }

        protected IHttpResponse File(byte[] content)
        {
            this.Response.Headers.Add(new HttpHeader(HttpHeader.ContentLength, content.Length.ToString()));
            this.Response.Headers.Add(new HttpHeader(HttpHeader.ContentDisposition, "inline"));
            this.Response.Content = content;
            return this.Response;
        }

        protected IHttpResponse Redirect(string location)
        {
            this.Response.Headers.Add(new HttpHeader(HttpHeader.Location, location));
            this.Response.StatusCode = HttpResponseStatusCode.SeeOther;
            return this.Response;
        }

        protected IHttpResponse Text(string content)
        {
            this.Response.Headers.Add(new HttpHeader(HttpHeader.ContentType, "text/plain; charset=utf-8"));
            this.Response.Content = Encoding.UTF8.GetBytes(content);
            return this.Response;
        }

        protected IHttpResponse BadRequestError(string errorMessage)
        {
            var viewModel = new ErrorViewModel { Error = errorMessage };
            var allContent = this.GetViewContent("Error", viewModel);

            this.PrepareHtmlResult(allContent);
            this.Response.StatusCode = HttpResponseStatusCode.BadRequest;
            return this.Response;
        }

        protected IHttpResponse BadRequestErrorWithView(string errorMessage)
        {
            return this.BadRequestErrorWithView(errorMessage, (object)null);
        }

        protected IHttpResponse BadRequestErrorWithView<T>(string errorMessage, T model, string layoutName = "_Layout")
        {
            var errorContent = this.GetViewContent("Error", new ErrorViewModel() { Error = errorMessage }, null);

            var viewName = this.Request.Path.Trim('/', '\\');
            if (string.IsNullOrWhiteSpace(viewName))
            {
                viewName = "home/index";
            }

            var viewContent = this.GetViewContent(viewName, model, null);
            var allViewContent = errorContent + Environment.NewLine + viewContent;
            var errorAndViewContent = this.ViewEngine.GetHtml(viewName, allViewContent, model, this.User);

            var layoutFileContent = SIO.File.ReadAllText($"Views/{layoutName}.html");
            var allContent = layoutFileContent.Replace("@RenderBody()", errorAndViewContent);
            var layoutContent = this.ViewEngine.GetHtml(layoutName, allContent, model, this.User);

            this.PrepareHtmlResult(layoutContent);
            this.Response.StatusCode = HttpResponseStatusCode.BadRequest;
            return this.Response;
        }

        protected IHttpResponse ServerError(string errorMessage)
        {
            var viewModel = new ErrorViewModel { Error = errorMessage };
            var allContent = this.GetViewContent("Error", viewModel);

            this.PrepareHtmlResult(allContent);
            this.Response.StatusCode = HttpResponseStatusCode.InternalServerError;
            return this.Response;
        }

        private string GetViewContent<T>(string viewName, T model, string layoutName = "_Layout")
        {
            //var content = this.ViewEngine.GetHtml(viewName, SIO.File.ReadAllText("Views/" + viewName + ".html"), model, this.User);

            viewName = viewName.Contains("/") ? viewName.Substring(viewName.LastIndexOf('/') + 1).Capitalize() : viewName;

            var assemblyLocation = Assembly.GetEntryAssembly().Location.Replace('\\', '/');
            var rootDirectoryPath = assemblyLocation.Replace('\\', '/').Substring(0, assemblyLocation.LastIndexOf('/'));
            var viewsDirectoryPath = rootDirectoryPath + "/Views";

            var viewsDirectoryFiles = SIO.Directory.GetFiles(viewsDirectoryPath);
            var content = string.Empty;
            if (viewsDirectoryFiles.Any(f => f.EndsWith(viewName + ".html")))
            {
                content = this.ViewEngine.GetHtml(viewName, SIO.File.ReadAllText("Views/" + viewName + ".html"), model, this.User);

                if (layoutName == null)
                {
                    return content;
                }
            }
            else
            {
                var subDirectories = SIO.Directory.GetDirectories(viewsDirectoryPath);
                foreach (var subDirectory in subDirectories)
                {
                    var directoryName = subDirectory.Substring(subDirectory.LastIndexOf('\\') + 1);
                    var subDirectoryFiles = SIO.Directory.GetFiles(subDirectory);
                    if (subDirectoryFiles.Any(f => f.EndsWith(viewName + ".html")))
                    {
                        content = this.ViewEngine.GetHtml(viewName, SIO.File.ReadAllText($"Views/{directoryName}/" + viewName + ".html"), model, this.User);

                        if (layoutName == null)
                        {
                            return content;
                        }
                    }
                }
            }

            var layoutFileContent = SIO.File.ReadAllText($"Views/{layoutName}.html");
            var allContent = layoutFileContent.Replace("@RenderBody()", content);
            var layoutContent = this.ViewEngine.GetHtml("_Layout", allContent, model, this.User);
            return layoutContent;
        }

        private void PrepareHtmlResult(string content)
        {
            this.Response.Headers.Add(new HttpHeader(HttpHeader.ContentType, "text/html; charset=utf-8"));
            this.Response.Content = Encoding.UTF8.GetBytes(content);
        }
    }
}
