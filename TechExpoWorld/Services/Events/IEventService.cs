namespace TechExpoWorld.Services.Events
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TechExpoWorld.Services.Events.Models;

    public interface IEventService
    {
        Task<IEnumerable<EventServiceModel>> All();

        Task<EventDetailsServiceModel> Details(int eventId);

        Task<IEnumerable<MyTicketServiceModel>> MyPhysicalTickets(string attendeeId);

        Task<IEnumerable<MyTicketServiceModel>> MyVirtualTickets(string attendeeId);

        Task<int> CreateEventWithTickets(
            string title,
            string content,
            string location,
            DateTime? startDate,
            DateTime? endDate,
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
            DateTime? startDate,
            DateTime? endDate,
            int totalPhysicalTickets,
            decimal physicalTicketPrice,
            int totalVirtualTickets,
            decimal virtualTicketPrice);

        Task<bool> Delete(int eventId);

        Task<bool> BookPhysicalTicket(int eventId, string attendeeId);

        Task<bool> BookVirtualTicket(int eventId, string attendeeId);

        Task<bool> CancelTicket(int eventId, int ticketId, string attendeeId);

        Task<int> TotalAvailablePhysicalTickets(int eventId);

        Task<int> TotalAvailableVirtualTickets(int eventId);
    }
}
