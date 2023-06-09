using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SampleHotel.ViewModels
{
    public class AuthViewModel
    {
        public LoginViewModel LoginModel { get; set; } = new LoginViewModel();
        public RegisterViewModel RegisterModel { get; set; } = new RegisterViewModel();

    }

    public class LoginViewModel
    {
        [DisplayName("Username")]
        [Required]
        public string Username { get; set; }
        [DisplayName("Password")]
        [Required]
        public string Password { get; set; }
    }

    public class RegisterViewModel
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

        [Required]
        public int UserTypeId { get; set; }
    }
}
