using static System.Net.Mime.MediaTypeNames;

namespace SampleHotel.ViewModels
{
    public class CreateRoomViewModel
    {
        private IFormFile _image { get; set; }

        public string Name { get; set; }
        public string Location { get; set; }
        public string Floor { get; set; }
        public string Capacity { get; set; }
        public string ImageUrl { get; set; }
        public int HasJacuzzi { get; set; }
        public int HasBreakfast { get; set; }
        public int RoomTypeId { get; set; }

        public IFormFile Image
        {
            get => _image;
            set
            {
                ImageUrl = value.FileName;
                _image = value;
            }
        }
    }
}
