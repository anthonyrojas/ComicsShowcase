﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ComicsShowcaseV3.Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [DisplayName("username")]
        public string Username { get; set; }

        [Required]
        [DisplayName("email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DisplayName("firstName")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("lastName")]
        public string LastName { get; set; }

        [Required]
        [DisplayName("password")]
        public string Password { get; set; }

        [Required]
        [DisplayName("birthDate")]
        public string BirthDate { get; set; }

        [DisplayName("profileStr")]
        public string ProfileStr { get; set; }

        [DisplayName("profile")]
        public byte[] Profile { get; set; }

        [DisplayName("redditUsername")]
        public string redditUsername { get; set; }

        [DisplayName("instagramUsername")]
        public string instagramUsername { get; set; }
    }
}
