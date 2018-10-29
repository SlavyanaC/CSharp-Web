namespace MishMashWebApp.Controllers
{
    using System.Linq;
    using SIS.HTTP.Responses.Contracts;
    using ViewModels.Channel;
    using Models;
    using Models.Enums;
    using ViewModels.Home;

    public class HomeController : BaseController
    {
        public IHttpResponse Index()
        {
            var user = this.DbContext.Users.FirstOrDefault(u => u.Username == this.User);
            if (user != null)
            {
                var loggedInUserHomeViewModel = GetLoggedInUserHomeViewModel(user);
                return user.Role == UserRole.Admin ? this.View("UserHomeIndex", loggedInUserHomeViewModel, "_LayoutAdmin")
                    : this.View("UserHomeIndex", loggedInUserHomeViewModel);
            }

            return this.View();
        }

        private LoggedInUserHomeViewModel GetLoggedInUserHomeViewModel(User user)
        {
            var followedChannels = this.DbContext.Channels
                .Where(c => c.Followers.Any(f => f.User.Username == user.Username))
                .Select(c => new ChannelViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Type = c.Type,
                    FollowersCount = c.Followers.Count(),
                }).ToArray();

            var followedChannelsTags = this.DbContext.Channels
                .Where(c => c.Followers.Any(f => f.User.Username == user.Username))
                .SelectMany(c => c.Tags.Select(t => t.TagId)).ToArray();

            var suggestedChannels = this.DbContext.Channels
                .Where(c => c.Followers.All(f => f.User.Username != user.Username) &&
                            c.Tags.Any(t => followedChannelsTags.Contains(t.TagId)))
                .Select(c => new ChannelViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Type = c.Type,
                    FollowersCount = c.Followers.Count(),
                }).ToArray();

            var followedAndSuggestedChannelsIds = followedChannels.Select(c => c.Id).ToArray()
                .Concat(suggestedChannels.Select(c => c.Id).ToArray()).Distinct();

            var seeOtherChannels = this.DbContext.Channels
                .Where(c => !followedAndSuggestedChannelsIds.Contains(c.Id))
                .Select(c => new ChannelViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Type = c.Type,
                    FollowersCount = c.Followers.Count(),
                })
                .ToArray();

            var loggedInUserHomeViewModel = new LoggedInUserHomeViewModel
            {
                Role = user.Role,
                FollowedChannels = followedChannels,
                SuggestedChannels = suggestedChannels,
                SeeOtherChannelViewModels = seeOtherChannels,
            };
            return loggedInUserHomeViewModel;
        }
    }
}
