namespace SampleHotel.ViewModels
{
    public class CreateEmployeeViewModel
    {
        private IFormFile _image { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int EmployeePositionId { get; set; }
        public IFormFile Image
        {
            get => _image;
            set
            {
                ImageUrl = value.FileName;
                _image = value;
            }
        }

        public string ImageUrl { get; set; }

        public string DependentFirstName { get; set; }
        public string DependentLastName { get; set; }
        public string DependentRelation { get; set; }
    }
}
