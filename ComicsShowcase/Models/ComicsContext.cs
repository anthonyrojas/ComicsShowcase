using System;
using Microsoft.EntityFrameworkCore;
namespace ComicsShowcase.Models
{
    public class ComicsContext : DbContext
    {
        public ComicsContext(DbContextOptions<ComicsContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<ComicBook> Comics { get; set; }
        public DbSet<Creator> Creators { get; set; }
        public DbSet<GraphicNovel> GraphicNovels { get; set;}
        public DbSet<Collectible> Collectibles { get; set; }
        public DbSet<Movie> Movies { get; set; }
    }
}
