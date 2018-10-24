namespace MishMashWebApp.ViewModels.Channel
{
    using Models.Enums;

    public class FollowedChannelViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ChannelType Type { get; set; }

        public int FollowersCount { get; set; }
    }
}
