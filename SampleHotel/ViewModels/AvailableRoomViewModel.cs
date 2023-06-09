using SampleHotel.Models;

namespace SampleHotel.ViewModels
{
    public class AvailableRoomViewModel
    {
        public IEnumerable<RoomDto> RoomList { get; set; } = new List<RoomDto>();
        public RoomSearchParameters SearchParameters { get; set; } = new RoomSearchParameters();
    }

    public class RoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int Floor { get; set; }
        public int Capacity { get; set; }
        public string ImageUrl { get; set; }
        public bool HasJacuzzi { get; set; }
        public bool HasBreakfast { get; set; }
        public int RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public bool IsLiked  { get; set; }
    }    

    public class RoomSearchParameters
    {
        public DateTime? From { get; set; } = DateTime.Now.Date;
        public DateTime? To { get; set; } = DateTime.Now.AddDays(1).Date;
        public int? HasJacuzzi { get; set; }
        public int? HasBreakfast { get; set; }
        public int Capacity { get; set; }
        public int RoomTypeId { get; set; }
    }
}
