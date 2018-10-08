namespace IRunesData
{
    using Microsoft.EntityFrameworkCore;
    using IRunesModels;

    public class IRunesContext : DbContext
    {
        public DbSet<Album> Albums { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<TrackAlbum> TrackAlbums { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAlbum> UserAlbums { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLazyLoadingProxies()
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAlbum>(e => e.HasKey(x => new { x.UserId, x.AlbumId }));
            modelBuilder.Entity<TrackAlbum>(e => e.HasKey(x => new { x.TrackId, x.AlbumId }));
        }
    }
}
