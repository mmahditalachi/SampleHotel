namespace SampleHotel.ViewModels
{
    public class RoomViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int Floor { get; set; }
        public int Capacity { get; set; }
        public string ImageUrl { get; set; }
        public bool HasJacuzzi { get; set; }
        public bool HasBreakfast { get; set; }
        public DateTime From { get; set; } = DateTime.Now;
        public DateTime To { get; set; } = DateTime.Now.AddDays(1);
    }
}
