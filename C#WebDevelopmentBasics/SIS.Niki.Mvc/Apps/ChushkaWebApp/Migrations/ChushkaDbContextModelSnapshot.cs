﻿// <auto-generated />
using System;
using ChushkaWebApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ChushkaWebApp.Migrations
{
    [DbContext(typeof(ChushkaDbContext))]
    partial class ChushkaDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ChushkaWebApp.Models.Order", b =>
                {
                    b.Property<int>("ProductId");

                    b.Property<int>("ClientId");

                    b.Property<DateTime>("OrderedOn");

                    b.HasKey("ProductId", "ClientId");

                    b.HasIndex("ClientId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("ChushkaWebApp.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<decimal>("Price");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("ChushkaWebApp.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email");

                    b.Property<string>("FullName");

                    b.Property<string>("Password");

                    b.Property<int>("Role");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ChushkaWebApp.Models.Order", b =>
                {
                    b.HasOne("ChushkaWebApp.Models.User", "Client")
                        .WithMany("Orders")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ChushkaWebApp.Models.Product", "Product")
                        .WithMany("Orders")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
