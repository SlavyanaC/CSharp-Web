namespace MishMashWebApp.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using MishMashWebApp.ViewModels.Channel;
    using SIS.HTTP.Responses.Contracts;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Attributes;

    public class ChannelController : BaseController
    {
        [HttpGet("/channels/followed")]
        public IHttpResponse MyChannels()
        {
            if (this.User == null)
            {
                return this.Redirect("/users/login");
            }

            var followedChannels = this.DbContext.Channels
                .Where(c => c.Followers.Any(f => f.User.Username == this.User))
                .Select(c => new FollowedChannelViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Type = c.Type,
                    FollowersCount = c.Followers.Count(),
                }).ToArray();

            return this.View("Followed", followedChannels);
        }

        [HttpGet("/channels/unfollow")]
        public IHttpResponse Unfollow(int id)
        {
            if (this.User == null)
            {
                return this.Redirect("/users/login");
            }

            var userChannel = this.DbContext.UserChannels.FirstOrDefault(uc => uc.User.Username == this.User && uc.ChannelId == id);
            if (userChannel == null)
            {
                return this.BadRequestError("You are not following this channel.");
            }

            this.DbContext.UserChannels.Remove(userChannel);
            this.DbContext.SaveChanges();

            return this.Redirect("/channels/followed");
        }

        [HttpGet("/channels/details")]
        public IHttpResponse Details(int id)
        {
            if (this.User == null)
            {
                return this.Redirect("/users/login");
            }

            var channelViewModel = this.DbContext.Channels.Where(c => c.Id == id)
                .Select(c => new ChannelDetailsViewModel()
                {
                    Name = c.Name,
                    Type = c.Type,
                    Description = c.Description,
                    Tags = c.Tags.Select(ct => ct.Tag),
                    Followers = c.Followers.Count()

                }).FirstOrDefault();

            return this.View("Details", channelViewModel);
        }
    }
}
