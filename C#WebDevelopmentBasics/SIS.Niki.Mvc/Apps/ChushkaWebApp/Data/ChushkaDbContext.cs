﻿namespace ChushkaWebApp.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class ChushkaDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.ConnectionString).UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(e => e.HasKey(k => new { k.ProductId, k.ClientId }));
        }
    }
}