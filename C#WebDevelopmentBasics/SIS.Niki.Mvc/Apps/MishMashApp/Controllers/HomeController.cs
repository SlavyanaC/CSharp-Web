namespace MishMashWebApp.Controllers
{
    using SIS.HTTP.Responses.Contracts;
    using SIS.MvcFramework.Attributes;
    using System.Linq;
    using Models.Enums;
    using ViewModels.Channel;

    public class HomeController : BaseController
    {
        [HttpGet("/home/index")]
        public IHttpResponse Index()
        {
            if (this.User != null)
            {
                var user = this.DbContext.Users.FirstOrDefault(u => u.Username == this.User);

                if (user.Role == UserRole.User)
                {
                    var followedChannels = this.DbContext.Channels
                        .Where(c => c.Followers.Any(f => f.User.Username == this.User))
                        .Select(c => new FollowedChannelViewModel
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Type = c.Type,
                            FollowersCount = c.Followers.Count(),
                        }).ToArray();

                    return this.View("UserHomeIndex", followedChannels);
                }
                else
                {
                    return this.View("AdminHomeIndex");
                }
            }

            return this.View("HomeIndex");

        }

        [HttpGet("/")]
        public IHttpResponse RootIndex()
        {
            return this.Index();
        }
    }
}
