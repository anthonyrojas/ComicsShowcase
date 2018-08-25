using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ComicsShowcase.Models
{
    public class Collectible
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public Byte[] ImageData { get; set; }
        public long UPC { get; set; }
        [Required]
        public bool Autographed { get; set; }
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
