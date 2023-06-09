using SampleHotel.Models;

namespace SampleHotel.ViewModels
{
    public class TicketListViewModel
    {
        public TicketListViewModel()
        {

        }

        public IEnumerable<TicketDto> TicketList { get; set; } = new List<TicketDto>();
        public Guid CurrentUserId { get; set; }
        public TicketMessageDto Message { get; set; } = new TicketMessageDto();
    }

    public class TicketDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Priority { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public string Description { get; set; }
        public Guid Creator { get; set; }

        public Guid SenderName { get; set; }

        public List<TicketMessageDto> TicketMessages { get; set; } = new List<TicketMessageDto>();
    }

    public class TicketMessageDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int TicketId { get; set; }
        public Guid Sender { get; set; }
        public DateTime Created { get; set; }
    }
}
