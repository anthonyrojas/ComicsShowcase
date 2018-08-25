using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace ComicsShowcase.Models
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
        [DisplayName("birthMonth")]
        public int BirthMonth { get; set; }
        [Required]
        [DisplayName("birthDate")]
        public int BirthDate { get; set; }
        [Required]
        [DisplayName("birthYear")]
        public int BirthYear { get; set; }
        [DisplayName("profile")]
        public Byte[] Profile { get; set; }
    }
}
