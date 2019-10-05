using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace ComicsShowcase.Models
{
    public class GraphicNovel
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [DisplayName("isbn")]
        public long ISBN { get; set; }

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

        [Required]
        [DisplayName("graphicNovelType")]
        public GraphicNovelType GraphicNovelType { get; set; }

        [Required]
        [DisplayName("bookCondition")]
        public BookCondition BookCondition { get; set; }

        [Required]
        [DisplayName("publisher")]
        public Publisher Publisher { get; set; }

        [Required]
        [DisplayName("user")]
        public User User { get; set; }

        [Required]
        [DisplayName("creators")]
        public ICollection<Creator> Creators { get; set; }
    }

    public enum GraphicNovelType{
        Paperback,
        Omnibus,
        Hardcover,
        DeluxeEdition
    }
    public enum BookCondition {
        New,
        Mint,
        Good,
        Poor
    }
}
