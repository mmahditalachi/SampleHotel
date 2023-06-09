namespace SampleHotel.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string Street { get; set; }

        public string EmployeePositionName { get; set; }
        public int EmployeePositionId { get; set; }

        public string ImageUrl { get; set; }
    }
}
