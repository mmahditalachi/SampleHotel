namespace SampleHotel.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string Street { get; set; }

        public EmployeePosition EmployeePosition { get; set; }
        public int EmployeePositionId { get; set; }

        public string ImageUrl { get; set; }


        public string DependentFirstName { get; set; }
        public string DependentLastName { get; set; }
        public string DependentRelation { get; set; }
    }
}
