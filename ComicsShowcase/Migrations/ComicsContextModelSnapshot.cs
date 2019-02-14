﻿// <auto-generated />
using System;
using ComicsShowcase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ComicsShowcase.Migrations
{
    [DbContext(typeof(ComicsContext))]
    partial class ComicsContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.2-rtm-30932")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ComicsShowcase.Models.Collectible", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Autographed");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<byte[]>("ImageData");

                    b.Property<string>("ImageStr");

                    b.Property<string>("ItemCategory")
                        .IsRequired();

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<long>("UPC");

                    b.Property<int?>("UserID");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("Collectibles");
                });

            modelBuilder.Entity("ComicsShowcase.Models.ComicBook", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Conidition")
                        .IsRequired();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<int>("FiveDigitId");

                    b.Property<byte[]>("ImageData");

                    b.Property<string>("ImageStr");

                    b.Property<string>("Publisher")
                        .IsRequired();

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<long>("UPC");

                    b.Property<int>("UserID");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("Comics");
                });

            modelBuilder.Entity("ComicsShowcase.Models.Creator", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ComicBookID");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<int?>("GraphicNovelID");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<int>("UserID");

                    b.HasKey("ID");

                    b.HasIndex("ComicBookID");

                    b.HasIndex("GraphicNovelID");

                    b.HasIndex("UserID");

                    b.ToTable("Creators");
                });

            modelBuilder.Entity("ComicsShowcase.Models.GraphicNovel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BookCondition")
                        .IsRequired();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<int>("GraphicNovelType");

                    b.Property<long>("ISBN");

                    b.Property<byte[]>("ImageData");

                    b.Property<string>("ImageStr");

                    b.Property<string>("Publisher")
                        .IsRequired();

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<int>("UserID");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("GraphicNovels");
                });

            modelBuilder.Entity("ComicsShowcase.Models.Movie", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("DiskType")
                        .IsRequired();

                    b.Property<byte[]>("ImageData");

                    b.Property<string>("ImageStr");

                    b.Property<int>("ReleaseYear");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<long>("UPC");

                    b.Property<int>("UserID");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("ComicsShowcase.Models.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BirthDate")
                        .IsRequired();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<byte[]>("Profile");

                    b.Property<string>("ProfileStr");

                    b.Property<string>("Username")
                        .IsRequired();

                    b.Property<string>("instagramUsername");

                    b.Property<string>("redditUsername");

                    b.HasKey("ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ComicsShowcase.Models.Collectible", b =>
                {
                    b.HasOne("ComicsShowcase.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID");
                });

            modelBuilder.Entity("ComicsShowcase.Models.ComicBook", b =>
                {
                    b.HasOne("ComicsShowcase.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ComicsShowcase.Models.Creator", b =>
                {
                    b.HasOne("ComicsShowcase.Models.ComicBook")
                        .WithMany("Creators")
                        .HasForeignKey("ComicBookID");

                    b.HasOne("ComicsShowcase.Models.GraphicNovel")
                        .WithMany("Creators")
                        .HasForeignKey("GraphicNovelID");

                    b.HasOne("ComicsShowcase.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ComicsShowcase.Models.GraphicNovel", b =>
                {
                    b.HasOne("ComicsShowcase.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ComicsShowcase.Models.Movie", b =>
                {
                    b.HasOne("ComicsShowcase.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
