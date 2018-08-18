using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ComicsShowcase.Models
{
    public class ComicBook
    {
        [Key]
        public int ComicId { get; set; }
        [Required]
        public string Title { get; set; }
        public long UPC { get; set; }
        public int FiveDigitIdentifier { get; set; }
        public Byte[] ImageData { get; set; }
        public Publisher Publisher { get; set; }
        [Required]
        public ComicCondition Conidition { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
        public ICollection<Creator> Creators { get; set; }
    }

    public enum ComicCondition{
        New,
        Mint,
        NearMint,
        Good,
        Poor
    }
}
