﻿using SIS.MvcFramework.ViewEngine;

namespace SIS.MvcFramework
{
    using System.Collections.Generic;
    using System.Text;
    using SIO = System.IO;
    using HTTP.Headers;
    using HTTP.Responses;
    using HTTP.Enums;
    using HTTP.Requests.Contracts;
    using HTTP.Responses.Contracts;
    using ViewEngine.Contracts;
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

        protected string User
        {
            // TODO: unify this
            get
            {
                if (!this.Request.Cookies.ContainsCookie(".auth-cakes"))
                {
                    return null;
                }

                var cookie = this.Request.Cookies.GetCookie(".auth-cakes");
                var cookieContent = cookie.Value;
                var userName = this.UserCookieService.GetUserData(cookieContent);
                return userName;
            }
        }

        protected IHttpResponse View(string viewName, string layoutName = "_Layout")
        {
            var allContent = this.GetViewContent(viewName, (object)null, layoutName);
            this.PrepareHtmlResult(allContent);
            return this.Response;
        }

        protected IHttpResponse View<T>(string viewName, T model = null, string layoutName = "_Layout")
            where T : class
        {
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
            var viewModel = new ErrorViewContent { Error = errorMessage };
            var allContent = this.GetViewContent("Error", viewModel);

            this.PrepareHtmlResult(allContent);
            this.Response.StatusCode = HttpResponseStatusCode.BadRequest;
            return this.Response;
        }

        protected IHttpResponse ServerError(string errorMessage)
        {
            var viewModel = new ErrorViewContent { Error = errorMessage };
            var allContent = this.GetViewContent("Error", viewModel);

            this.PrepareHtmlResult(allContent);
            this.Response.StatusCode = HttpResponseStatusCode.InternalServerError;
            return this.Response;
        }

        private string GetViewContent<T>(string viewName, T model, string layoutName = "_Layout")
        {
            var content = this.ViewEngine.GetHtml(viewName, SIO.File.ReadAllText("Views/" + viewName + ".html"), model,
                this.User);

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
