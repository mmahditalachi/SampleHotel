namespace SampleHotel.ViewModels
{
    public class ReserveRoomListViewModel
    {
        public IEnumerable<ReserveRoomDto> ReserveRooms { get; set; } = new List<ReserveRoomDto>();
    }

    public class ReserveRoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int Floor { get; set; }
        public int Capacity { get; set; }
        public string ImageUrl { get; set; }
        public bool HasJacuzzi { get; set; }
        public bool HasBreakfast { get; set; }
        public int RoomReserveId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
