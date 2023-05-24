namespace TechExpoWorld.Web.ViewModels.Events
{
    public class EventDetailsViewModel : EventViewModel
    {
        public string Content { get; init; }

        public int TotalPhysicalTickets { get; init; }

        public int TotalVirtualTickets { get; init; }

        public int TotalAvailablePhysicalTickets { get; init; }

        public int TotalAvailableVirtualTickets { get; init; }

        public decimal PhysicalTicketPrice { get; set; }

        public decimal VirtualTicketPrice { get; set; }
    }
}
