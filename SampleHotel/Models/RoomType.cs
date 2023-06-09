namespace SampleHotel.Models
{
    public class RoomType : Enumeration
    {
        public static RoomType Luxury = new RoomType(1, "Luxury");
        public static RoomType Standard = new RoomType(2, "Standard");
        public static RoomType Suite = new RoomType(3, "Suite");

        public RoomType(int id, string name) 
            : base(id, name)
        {
        }
    }
}
