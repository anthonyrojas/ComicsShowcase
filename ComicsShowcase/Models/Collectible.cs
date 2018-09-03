using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace ComicsShowcase.Models
{
    public class Collectible
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [DisplayName("title")]
        public string Title { get; set; }

        [Required]
        [DisplayName("description")]
        public string Description { get; set; }

        [DisplayName("imageStr")]
        public string ImageStr { get; set; }

        [DisplayName("imageData")]
        public byte[] ImageData { get; set; }

        [DisplayName("upc")]
        public long UPC { get; set; }

        [Required]
        [DisplayName("autographed")]
        public bool Autographed { get; set; }

        [Required]
        [DisplayName("itemCategory")]
        public CollectibleCategory ItemCategory { get; set; }

        [DisplayName("user")]
        public User User { get; set; }
    }

    public enum CollectibleCategory{
        Poster,
        ActionFigure,
        FunkoPop,
        Script,
        Other
    }
}
