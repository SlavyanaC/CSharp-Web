namespace MishMashWebApp.Controllers
{
    using System;
    using System.Linq;
    using SIS.MvcFramework.Attributes;
    using SIS.HTTP.Responses.Contracts;
    using ViewModels.Channel;
    using Models;
    using Models.Enums;

    public class ChannelsController : BaseController
    {
        [Authorize]
        public IHttpResponse Followed()
        {
            var user = this.DbContext.Users.FirstOrDefault(u => u.Username == this.User);

            var followedChannels = this.DbContext.Channels
                .Where(c => c.Followers.Any(f => f.User.Username == this.User))
                .Select(c => new ChannelViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Type = c.Type,
                    FollowersCount = c.Followers.Count(),
                }).ToArray();

            return user.Role == UserRole.Admin ? this.View(followedChannels, "_LayoutAdmin")
                : this.View(followedChannels);
        }

        [Authorize]
        public IHttpResponse Follow(int id)
        {
            var user = this.DbContext.Users.FirstOrDefault(u => u.Username == this.User);
          
            var channel = this.DbContext.Channels.FirstOrDefault(c => c.Id == id);
            if (channel == null)
            {
                return this.BadRequestErrorWithView($"Channel with id {id} not found.");
            }

            var userChannel = new UserChannel()
            {
                UserId = user.Id,
                ChannelId = channel.Id,
            };

            this.DbContext.UserChannels.Add(userChannel);
            try
            {
                this.DbContext.SaveChanges();
            }
            catch (Exception e)
            {
                return this.ServerError(e.Message);
            }

            return this.Redirect("/");
        }

        [Authorize]
        public IHttpResponse Unfollow(int id)
        {
           var userChannel = this.DbContext.UserChannels.FirstOrDefault(uc => uc.User.Username == this.User && uc.ChannelId == id);
            if (userChannel == null)
            {
                return this.BadRequestErrorWithView("You are not following this channel.");
            }

            this.DbContext.UserChannels.Remove(userChannel);
            this.DbContext.SaveChanges();

            return this.Redirect("/channels/followed");
        }

        [Authorize]
        public IHttpResponse Details(int id)
        {
            var user = this.DbContext.Users.FirstOrDefault(u => u.Username == this.User);
           
            var channelViewModel = this.DbContext.Channels.Where(c => c.Id == id)
                .Select(c => new ChannelDetailsViewModel()
                {
                    Name = c.Name,
                    Type = c.Type,
                    Description = c.Description,
                    Tags = c.Tags.Select(ct => ct.Tag),
                    Followers = c.Followers.Count()

                }).FirstOrDefault();

            if (channelViewModel == null)
            {
                return this.BadRequestError("Invalid channel id.");
            }

            return user.Role == UserRole.Admin ? this.View(channelViewModel, "_LayoutAdmin")
                : this.View(channelViewModel);
        }

        [Authorize]
        public IHttpResponse Create()
        {
            var user = this.DbContext.Users.FirstOrDefault(u => u.Username == this.User);
            if (user.Role != UserRole.Admin)
            {
                return Redirect("/");
            }

            return this.View(layoutName: "_LayoutAdmin");
        }

        [HttpPost]
        [Authorize]
        public IHttpResponse Create(CreateChannelViewModel model)
        {
            var user = this.DbContext.Users.FirstOrDefault(u => u.Username == this.User);
            if (user.Role != UserRole.Admin)
            {
                return this.BadRequestErrorWithView("You have no permission to access this page.");
            }

            if (model.Name == null)
            {
                return this.BadRequestErrorWithView("A channel must have name.");
            }

            if (!Enum.TryParse(model.Type, out ChannelType type))
            {
                return this.BadRequestErrorWithView("Please select channel type.");
            }

            var channel = new Channel()
            {
                Name = model.Name,
                Type = type,
                Description = model.Description,
            };

            if (!string.IsNullOrWhiteSpace(model.Tags))
            {
                var tags = model.Tags
                    .Split(new[] { ';', ',', ' ', }, StringSplitOptions.RemoveEmptyEntries)
                    .ToArray();

                foreach (var tagName in tags)
                {
                    var tag = this.DbContext.Tags.FirstOrDefault(t => t.Name == tagName);
                    if (tag == null)
                    {
                        tag = new Tag() { Name = tagName.Trim() };
                        this.DbContext.Tags.Add(tag);
                        try
                        {
                            this.DbContext.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            return this.ServerError(e.Message);
                        }
                    }

                    channel.Tags.Add(new ChannelTag() { TagId = tag.Id });
                }
            }

            this.DbContext.Channels.Add(channel);

            try
            {
                this.DbContext.SaveChanges();
            }
            catch (Exception e)
            {
                return this.ServerError(e.Message);
            }

            return this.Redirect("/channels/details?id=" + channel.Id);
        }
    }
}
