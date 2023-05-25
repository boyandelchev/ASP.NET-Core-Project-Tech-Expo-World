namespace TechExpoWorld.Web.ViewModels.Events
{
    public class EventDetailsViewModel : EventViewModel
    {
        public string Content { get; init; }

        public int TotalPhysicalTickets { get; init; }

        public int TotalVirtualTickets { get; init; }

        public int TotalAvailablePhysicalTickets { get; set; }

        public int TotalAvailableVirtualTickets { get; set; }

        public decimal PhysicalTicketPrice { get; set; }

        public decimal VirtualTicketPrice { get; set; }
    }
}
