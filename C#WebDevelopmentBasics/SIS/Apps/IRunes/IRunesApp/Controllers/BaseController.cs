namespace IRunesApp.Controllers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Services;
    using Services.Contracts;
    using IRunesData;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;

    public abstract class BaseController
    {
        private const string ROOT_DIRECTORY_RELATIVE_PATH = "../../../";
        private const string VIEWS_FOLDER_NAME = "Views";
        private const string CONTROLLER_DEFAULT_NAME = "Controller";
        private const string DIRECTORY_SEPARATOR = "/";
        private const string HTML_FILE_EXTENSION = ".html";
        private readonly string[] notLoggedPaths = { "Login", "Register", "Index" };

        protected BaseController()
        {
            this.Db = new IRunesContext();
            this.UserCookieService = new UserCookieService();
            this.ViewBag = new Dictionary<string, string>();
        }

        protected IRunesContext Db { get; set; }

        public IUserCookieService UserCookieService { get; set; }

        protected IDictionary<string, string> ViewBag { get; set; }

        protected bool IsAuthenticated(IHttpRequest request)
        {
            return request.Session.ContainsParameter("username");
        }

        protected IHttpResponse View([CallerMemberName] string viewName = "")
        {
            var filePath = ROOT_DIRECTORY_RELATIVE_PATH +
                              VIEWS_FOLDER_NAME +
                              DIRECTORY_SEPARATOR +
                              this.GetCurrentControllerName() +
                              DIRECTORY_SEPARATOR + viewName +
                              HTML_FILE_EXTENSION;

            var layoutFilePath = GetLayoutFilePath(viewName);

            if (!File.Exists(filePath))
            {
                return this.BadRequestError($"View {viewName} not found");
            }

            var fileContent = File.ReadAllText(filePath);
            var layoutFileContent = File.ReadAllText(layoutFilePath);

            foreach (var viewBagKey in ViewBag.Keys)
            {
                var variable = $"@{viewBagKey}";

                if (fileContent.Contains(viewBagKey))
                {
                    fileContent = fileContent.Replace(variable, this.ViewBag[viewBagKey]);
                }
            }

            var fullHtmlFile = layoutFileContent.Replace("@contentBody", fileContent);

            var response = new HtmlResult(fullHtmlFile, HttpResponseStatusCode.Ok);
            return response;
        }

        protected IHttpResponse BadRequestError(string errorMessage)
        {
            var content = $"<h1>{errorMessage}</h1>";
            return new HtmlResult(content, HttpResponseStatusCode.BadRequest);
        }

        protected IHttpResponse ServerError(string errorMessage)
        {
            var content = $"<h1>{errorMessage}</h1>";
            return new HtmlResult(content, HttpResponseStatusCode.InternalServerError);
        }

        private string GetLayoutFilePath(string viewName)
        {
            string layoutFilePath;

            if (this.notLoggedPaths.Contains(viewName))
            {
                layoutFilePath = ROOT_DIRECTORY_RELATIVE_PATH +
                                 VIEWS_FOLDER_NAME +
                                 DIRECTORY_SEPARATOR +
                                 "_layoutNotLogged" +
                                 HTML_FILE_EXTENSION;
            }
            else
            {
                layoutFilePath = ROOT_DIRECTORY_RELATIVE_PATH +
                                 VIEWS_FOLDER_NAME +
                                 DIRECTORY_SEPARATOR +
                                 "_layoutLogged" +
                                 HTML_FILE_EXTENSION;
            }

            return layoutFilePath;
        }

        protected string GetUsername(IHttpRequest request)
        {
            if (!request.Cookies.ContainsCookie(".auth-irunes"))
            {
                return null;
            }

            var cookie = request.Cookies.GetCookie(".auth-irunes");
            var cookieContent = cookie.Value;
            var username = this.UserCookieService.GetUserData(cookieContent);
            return username;
        }

        private string GetCurrentControllerName()
        {
            return this.GetType().Name.Replace(CONTROLLER_DEFAULT_NAME, string.Empty);
        }
    }
}
