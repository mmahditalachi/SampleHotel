namespace SampleHotel.Models
{
    public class RoomReserve
    {
        public int Id { get; set; }
        public Room Room { get; set; }
        public int RoomId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public Guid UserId { get; set; }
    }
}
