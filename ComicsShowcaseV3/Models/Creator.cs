using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ComicsShowcaseV3.Models
{
    public class Creator
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [DisplayName("firstName")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("lastName")]
        public string LastName { get; set; }

        [Required]
        [DisplayName("user")]
        public User User { get; set; }
    }

    public enum Role{
        Writer,
        Artist,
        Inker,
        All
    }

}
