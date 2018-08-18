using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ComicsShowcase.Models
{
    public class Creator
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public Role Role { get; set; }
        [Required]
        public User User { get; set; }
    }

    public enum Role{
        Writer,
        Artist,
        Inker,
        All
    }
}
