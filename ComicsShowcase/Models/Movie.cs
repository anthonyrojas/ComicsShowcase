using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ComicsShowcase.Models
{
    public class Movie
    {
        [Key]
        public long UPC { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public Byte[] ImageData { get; set; }
        [Required]
        public int ReleaseYear { get; set; }
        [Required]
        public DiskType DiskType { get; set; }
        public User User { get; set; }
    }

    public enum DiskType
    {
        DVD,
        BluRay,
        BluRay4K
    }
}
