namespace TechExpoWorld.Models.Events
{
    using TechExpoWorld.Services.Events.Models;

    public class EventDetailsViewModel
    {
        public EventDetailsServiceModel EventDetails { get; init; }

        public int TotalAvailablePhysicalTickets { get; init; }

        public int TotalAvailableVirtualTickets { get; init; }
    }
}
