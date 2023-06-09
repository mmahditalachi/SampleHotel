using Microsoft.AspNetCore.Identity;

namespace SampleHotel.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FullName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string No { get; set; }
    }
}
