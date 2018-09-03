using System;
using Microsoft.EntityFrameworkCore;
namespace ComicsShowcase.Models
{
    public class ComicsContext : DbContext
    {

        public ComicsContext(DbContextOptions<ComicsContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Creator>().Property(x => x.Role).HasDefaultValue(Role.Writer);
            modelBuilder
                .Entity<ComicBook>()
                .Property(e => e.Conidition)
                .HasConversion(
                v => v.ToString(),
                    v => (ComicCondition)Enum.Parse(typeof(ComicCondition), v));
            modelBuilder.Entity<ComicBook>().Property(e => e.Publisher).HasConversion(v => v.ToString(),v => (Publisher)Enum.Parse(typeof(Publisher), v));
            modelBuilder.Entity<GraphicNovel>().Property(e => e.Publisher).HasConversion(v => v.ToString(), v => (Publisher)Enum.Parse(typeof(Publisher), v));
            modelBuilder.Entity<GraphicNovel>().Property(e => e.BookCondition).HasConversion(v => v.ToString(), v => (BookCondition)Enum.Parse(typeof(BookCondition), v));
            modelBuilder.Entity<Collectible>().Property(e => e.ItemCategory).HasConversion(v => v.ToString(), v => (CollectibleCategory)Enum.Parse(typeof(CollectibleCategory), v));
            modelBuilder.Entity<Movie>().Property(e => e.DiskType).HasConversion(v => v.ToString(), v => (DiskType)Enum.Parse(typeof(DiskType), v));
        }
        public DbSet<User> Users { get; set; }
        public DbSet<ComicBook> Comics { get; set; }
        public DbSet<Creator> Creators { get; set; }
        public DbSet<GraphicNovel> GraphicNovels { get; set; }
        public DbSet<Collectible> Collectibles { get; set; }
        public DbSet<Movie> Movies { get; set; }
    }
}
