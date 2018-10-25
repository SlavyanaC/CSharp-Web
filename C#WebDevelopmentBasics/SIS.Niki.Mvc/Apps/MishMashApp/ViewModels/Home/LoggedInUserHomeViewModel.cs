namespace MishMashWebApp.ViewModels.Home
{
    using System.Collections.Generic;
    using Models.Enums;
    using Channel;

    public class LoggedInUserHomeViewModel
    {
        public UserRole Role { get; set; }

        public IEnumerable<ChannelViewModel> FollowedChannels { get; set; }

        public IEnumerable<ChannelViewModel> SuggestedChannels { get; set; }

        public IEnumerable<ChannelViewModel> SeeOtherChannelViewModels { get; set; }
    }
}
