namespace TechExpoWorld.Services.Events.Models
{
    public class EventDetailsServiceModel : EventServiceModel
    {
        public string Content { get; init; }

        public int TotalPhysicalTickets { get; init; }

        public int TotalVirtualTickets { get; init; }

        public decimal PhysicalTicketPrice { get; set; }

        public decimal VirtualTicketPrice { get; set; }
    }
}
