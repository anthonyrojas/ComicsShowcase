using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace ComicsShowcase.Models
{
    public class Movie
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [DisplayName("upc")]
        public long UPC { get; set; }

        [Required]
        [DisplayName("title")]
        public string Title { get; set; }

        [DisplayName("description")]
        public string Description { get; set; }

        [DisplayName("imageStr")]
        public string ImageStr { get; set; }

        [DisplayName("imageData")]
        public byte[] ImageData { get; set; }

        [Required]
        [DisplayName("releaseYear")]
        public int ReleaseYear { get; set; }

        [Required]
        [DisplayName("diskType")]
        public DiskType DiskType { get; set; }

        [Required]
        [DisplayName("user")]
        public User User { get; set; }
    }

    public enum DiskType
    {
        DVD,
        BluRay,
        BluRay4K
    }
}
