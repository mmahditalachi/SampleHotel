using SampleHotel.Models;

namespace SampleHotel.ViewModels
{
    public class EmployeeListViewModel
    {
        public IEnumerable<EmployeeDto> EmployeeList { get; set; } = new List<EmployeeDto>();
    }

    public class EmployeeDto
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

        public string DependentFirstName { get; set; }
        public string DependentLastName { get; set; }
        public string DependentRelation { get; set; }
    }
}
