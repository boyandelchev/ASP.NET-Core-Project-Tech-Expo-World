namespace TechExpoWorld.Services.Events
{
    public class EventDetailsServiceModel : EventServiceModel
    {
        public string Content { get; init; }

        public int TotalPhysicalTickets { get; init; }

        public int TotalVirtualTickets { get; init; }

        public decimal PhysicalTicketPrice { get; init; }

        public decimal VirtualTicketPrice { get; init; }
    }
}
