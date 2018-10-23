using System;
using System.IO;

namespace SIS.Framework.Controllers
{
    using System.Runtime.CompilerServices;
    using HTTP.Requests.Contracts;
    using ActionResults.Contracts;
    using ActionResults;
    using Utilities;
    using Models;
    using Views;
    using Security.Contracts;

    public abstract class Controller
    {
        protected Controller()
        {
            this.Model = new ViewModel();
            this.ViewEngine = new ViewEngine();
        }

        public IHttpRequest Request { get; set; }

        // TODO: Check if this returns null or throws an error when cast doesn't succeed 
        public IIdentity Identity => (IIdentity)this.Request.Session.GetParameter("auth");

        public ViewModel Model { get; set; }

        public ViewEngine ViewEngine { get; }

        public Model ModelState => new Model();

        protected IViewable View([CallerMemberName] string actionName = "")
        {
            var controllerName = ControllerUtilities.GetControllerName(this);
            var viewContent = null;

            try
            {
                viewContent = this.ViewEngine.GetViewContent(controllerName, actionName);
            }
            catch (FileNotFoundException e)
            {
                this.Model.Data["Error"] = e.Message;
                viewContent = this.ViewEngine.GetErrorContent();
            }

            var renderedContent = this.ViewEngine.RenderHtml(viewContent, this.Model.Data);
            var view = new View(renderedContent);
            var viewResult = new ViewResult(view);
            return viewResult;
        }

        protected IRedirectable RedirectToAction(string redirectUrl) => new RedirectResult(redirectUrl);

        protected void SignIn(IIdentity auth)
        {
            this.Request.Session.AddParameter("auth", auth);
        }

        protected void SignOut()
        {
            this.Request.Session.ClearParameters();
        }

        //Uncomment this if property Identity throws error
        //public IIdentity Identity()
        //{
        //    if (this.Request.Session.ContainsParameter("auth"))
        //    {
        //        return (IIdentity)this.Request.Session.GetParameter("auth");
        //    }

        //    return null;
        //}
    }
}
