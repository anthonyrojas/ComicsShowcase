using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ComicsShowcaseV3.Models
{
    public class LoginModel
    {
        [Required]
        [DisplayName("username")]
        public string Username { get; set; }
        [DisplayName("password")]
        [Required]
        public string Password { get; set; }
    }
}
