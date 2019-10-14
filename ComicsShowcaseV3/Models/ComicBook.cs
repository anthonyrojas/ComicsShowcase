using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace ComicsShowcaseV3.Models
{
    public class ComicBook
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [DisplayName("title")]
        public string Title { get; set; }

        [Required]
        [DisplayName("description")]
        public string Description { get; set; }

        [DisplayName("UPC")]
        public long UPC { get; set; }

        [DisplayName("fiveDigitId")]
        public int FiveDigitId { get; set; }

        [DisplayName("imageStr")]
        public string ImageStr { get; set; }

        [DisplayName("imageData")]
        public byte[] ImageData { get; set; }

        [Required]
        [DisplayName("publisher")]
        public Publisher Publisher { get; set; }

        [Required]
        [DisplayName("condition")]
        public ComicCondition Conidition { get; set; }

        [Required]
        [DisplayName("user")]
        public User User { get; set; }

        [Required]
        [DisplayName("creators")]
        public ICollection<Creator> Creators { get; set; }
    }

    public enum ComicCondition{
        New=0,
        Mint=1,
        NearMint=2,
        Good=3,
        Poor=4
    }
}
