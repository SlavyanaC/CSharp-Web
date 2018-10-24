namespace MishMashWebApp.Models
{
    using System.Collections.Generic;
    using Enums;

    public class Channel
    {
        public Channel()
        {
            this.Tags = new HashSet<ChannelTag>();
            this.UserChannels = new HashSet<UserChannel>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ChannelType Type { get; set; }

        public virtual ICollection<ChannelTag> Tags { get; set; }

        public virtual ICollection<UserChannel> UserChannels { get; set; }
    }
}
