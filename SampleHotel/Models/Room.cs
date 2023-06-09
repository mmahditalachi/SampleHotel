using System.ComponentModel.DataAnnotations.Schema;

namespace SampleHotel.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int Floor { get; set; }
        public int Capacity { get; set; }
        public string ImageUrl { get; set; }
        public bool HasJacuzzi { get; set; }
        public bool HasBreakfast { get; set; }

        public RoomType RoomType { get; set; }
        public int RoomTypeId { get; set; }
    }
}
