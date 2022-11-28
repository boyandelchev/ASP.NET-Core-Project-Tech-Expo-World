namespace TechExpoWorld.Services.Events
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TechExpoWorld.Services.Events.Models;

    public interface IEventService
    {
        Task<IEnumerable<EventServiceModel>> All();

        Task<EventDetailsServiceModel> Details(int eventId);

        Task<IEnumerable<MyTicketServiceModel>> MyPhysicalTickets(int attendeeId);

        Task<IEnumerable<MyTicketServiceModel>> MyVirtualTickets(int attendeeId);

        Task<int> CreateEventWithTickets(
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

        Task<bool> Edit(
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

        Task<bool> Delete(int eventId);

        Task<bool> BuyPhysicalTicket(int eventId, int attendeeId);

        Task<bool> BuyVirtualTicket(int eventId, int attendeeId);

        Task<bool> RevokeTicket(int eventId, int ticketId, int attendeeId);

        Task<bool> EventExists(int eventId);

        Task<int> TotalAvailablePhysicalTicketsForEvent(int eventId);

        Task<int> TotalAvailableVirtualTicketsForEvent(int eventId);
    }
}
