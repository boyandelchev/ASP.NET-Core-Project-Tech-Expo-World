namespace TechExpoWorld.Services.Events
{
    using System.Collections.Generic;
    using TechExpoWorld.Services.Events.Models;

    public interface IEventService
    {
        IEnumerable<EventServiceModel> All();

        EventDetailsServiceModel Details(int eventId);

        IEnumerable<MyTicketServiceModel> MyPhysicalTickets(int attendeeId);

        IEnumerable<MyTicketServiceModel> MyVirtualTickets(int attendeeId);

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

        bool BuyPhysicalTicket(int eventId, int attendeeId);

        bool BuyVirtualTicket(int eventId, int attendeeId);

        bool RevokeTicket(int eventId, int ticketId, int attendeeId);

        bool EventExists(int eventId);

        int TotalAvailablePhysicalTicketsForEvent(int eventId);

        int TotalAvailableVirtualTicketsForEvent(int eventId);
    }
}
