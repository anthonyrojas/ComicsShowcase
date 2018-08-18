using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ComicsShowcase.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public int BirthMonth { get; set; }
        [Required]
        public int BirthDate { get; set; }
        [Required]
        public int BirthYear { get; set; }
        public Byte[] ImageData { get; set; }
    }
}
