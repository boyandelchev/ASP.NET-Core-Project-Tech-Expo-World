namespace TechExpoWorld.Models.Events
{
    using TechExpoWorld.Services.Events.Models;

    public class EventDetailsViewModel
    {
        public EventDetailsServiceModel EventDetails { get; init; }

        public int TotalAvailablePhysicalTicketsForEvent { get; init; }

        public int TotalAvailableVirtualTicketsForEvent { get; init; }
    }
}
