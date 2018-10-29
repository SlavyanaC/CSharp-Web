namespace MishMashWebApp.ViewModels.Home
{
    using System.Collections.Generic;
    using Channel;

    public class LoggedInUserHomeViewModel
    {
        public IEnumerable<ChannelViewModel> FollowedChannels { get; set; }

        public IEnumerable<ChannelViewModel> SuggestedChannels { get; set; }

        public IEnumerable<ChannelViewModel> SeeOtherChannelViewModels { get; set; }
    }
}
