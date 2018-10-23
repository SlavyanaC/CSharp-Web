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
            this.ViewModel = new ViewModel();
        }

        public IHttpRequest Request { get; set; }

        // TODO: Check if this returns null or throws an error when cast doesn't succeed 
        public IIdentity Identity => (IIdentity)this.Request.Session.GetParameter("auth");

        public ViewModel ViewModel { get; set; }

        public Model ModelState => new Model();

        protected IViewable View([CallerMemberName] string caller = "")
        {
            var controllerName = ControllerUtilities.GetControllerName(this);
            var fullyQualifiedName = ControllerUtilities.GetViewFullyQualifiedName(controllerName, caller);

            var view = new View(fullyQualifiedName, this.ViewModel.Data);

            return new ViewResult(view);
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
