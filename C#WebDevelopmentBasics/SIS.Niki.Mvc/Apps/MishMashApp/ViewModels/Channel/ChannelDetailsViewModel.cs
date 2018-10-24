namespace MishMashWebApp.ViewModels.Channel
{
    using System.Collections.Generic;
    using Models;
    using Models.Enums;
    using System.Linq;

    public class ChannelDetailsViewModel
    {
        public string Name { get; set; }

        public ChannelType Type { get; set; }

        public int Followers { get; set; }

        public string Description { get; set; }

        public IEnumerable<Tag> Tags { get; set; }

        public string TagsAsString => string.Join(", ", this.Tags.Select(t => t.Name));
    }
}
