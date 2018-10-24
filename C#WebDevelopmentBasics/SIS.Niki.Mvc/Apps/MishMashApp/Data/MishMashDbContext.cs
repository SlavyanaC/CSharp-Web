namespace MishMashWebApp.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class MishMashDbContext : DbContext
    {
        public DbSet<Channel> Channels { get; set; }

        public DbSet<ChannelTag> ChannelTags { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserChannel> UserChannels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.ConnectionString).UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChannelTag>(e => e.HasKey(k => new { k.ChannelId, k.TagId }));
            modelBuilder.Entity<UserChannel>(e => e.HasKey(k => new { k.UserId, k.ChannelId }));
        }
    }
}
