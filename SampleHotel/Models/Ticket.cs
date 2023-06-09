namespace SampleHotel.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public TicketPriority Priority { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public string Description { get; set; }
        public Guid Creator { get; set; }        
    }

    public enum TicketPriority
    {
        Low,
        Medium, 
        High
    }

    public class TicketMessage
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public Ticket Ticket { get; set; }
        public int TicketId { get; set; }

        public Guid Sender { get; set; }

        public DateTime Created { get; set; }
    }
}



