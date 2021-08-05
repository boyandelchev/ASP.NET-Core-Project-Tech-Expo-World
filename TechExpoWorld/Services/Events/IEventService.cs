namespace TechExpoWorld.Services.Events
{
    using System.Collections.Generic;

    public interface IEventService
    {
        IEnumerable<EventServiceModel> All();

        EventDetailsServiceModel Details(int eventId);

        int CreateEventWithTickets(
            string title,
            string content,
            string location,
            string startDate,
            string endDate,
            int totalPhysicalTickets,
            decimal physicalTicketPrice,
            int totalVirtualTickets,
            decimal virtualTicketPrice,
            string userId);

        bool Edit(
            int eventId,
            string title,
            string content,
            string location,
            string startDate,
            string endDate,
            int totalPhysicalTickets,
            decimal physicalTicketPrice,
            int totalVirtualTickets,
            decimal virtualTicketPrice);

        bool Delete(int eventId);

        bool IsValidDate(string date);

        bool EventExists(int eventId);
    }
}
