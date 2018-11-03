namespace MeTubeWebApp.Controllers
{
    using System.Linq;
    using SIS.HTTP.Responses.Contracts;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Attributes;
    using ViewModels.Tubes;
    using Models;

    public class TubesController : BaseController
    {
        [Authorize]
        public IHttpResponse Details(int id)
        {
            var tube = this.DbContext.Tubes.Find(id);
            tube.Views++;
            this.DbContext.Tubes.Update(tube);
            this.DbContext.SaveChanges();
            return this.View(tube);
        }

        [Authorize]
        public IHttpResponse Upload()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public IHttpResponse Upload(TubeViewModel model)
        {
            var userId = this.DbContext.Users.FirstOrDefault(u => u.Username == this.User.Username).Id;
            var tube = model.To<Tube>();
            tube.UploaderId = userId;
            tube.YouTubeId = model.YouTubeId.Substring(model.YouTubeId.LastIndexOf("=") + 1);

            this.DbContext.Tubes.Add(tube);
            this.DbContext.SaveChanges();

            return this.Redirect($"/");
        }
    }
}
