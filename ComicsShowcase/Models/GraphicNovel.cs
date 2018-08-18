using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ComicsShowcase.Models
{
    public class GraphicNovel
    {
        [Key]
        public long ISBN { get; set; }
        [Required]
        public string Title { get; set; }
        public Byte[] ImageData { get; set; }
        [Required]
        public GraphicNovelType GraphicNovelType { get; set; }
        [Required]
        public BookCondition BookCondition { get; set; }
        [Required]
        public Publisher Publisher { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
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
