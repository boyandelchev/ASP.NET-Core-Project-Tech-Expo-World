namespace TechExpoWorld.Services.Events
{
    using System.Collections.Generic;

    public interface IEventService
    {
        IEnumerable<EventServiceModel> All();

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

        bool IsValidDate(string date);
    }
}
