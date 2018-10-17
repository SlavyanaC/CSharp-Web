namespace SIS.Framework.Controllers
{
    using System.Runtime.CompilerServices;
    using HTTP.Requests.Contracts;
    using ActionResults.Contracts;
    using ActionResults;
    using Utilities;
    using Models;
    using Views;

    public abstract class Controller
    {
        protected Controller()
        {
            this.ViewModel = new ViewModel();
        }

        public IHttpRequest Request { get; set; }

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
    }
}
