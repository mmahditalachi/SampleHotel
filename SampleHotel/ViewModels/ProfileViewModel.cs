using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SampleHotel.ViewModels
{
    public class ProfileViewModel
    {
        [DisplayName("FullName")]
        [Required]
        public string FullName { get; set; }

        [DisplayName("Username")]
        [Required]
        public string Username { get; set; }

        [DisplayName("Email")]
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [DisplayName("Password")]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string No { get; set; }
    }
}
