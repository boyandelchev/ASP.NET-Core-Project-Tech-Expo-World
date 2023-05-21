namespace TechExpoWorld.Services.Events
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TechExpoWorld.Services.Events.Models;

    public interface IEventsService
    {
        Task<IEnumerable<EventServiceModel>> AllAsync();

        Task<EventDetailsServiceModel> DetailsAsync(int eventId);

        Task<IEnumerable<MyTicketServiceModel>> MyPhysicalTicketsAsync(string attendeeId);

        Task<IEnumerable<MyTicketServiceModel>> MyVirtualTicketsAsync(string attendeeId);

        Task<int> CreateEventWithTicketsAsync(
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

        Task<bool> EditAsync(
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

        Task<bool> DeleteAsync(int eventId);

        Task<bool> BookPhysicalTicketAsync(int eventId, string attendeeId);

        Task<bool> BookVirtualTicketAsync(int eventId, string attendeeId);

        Task<bool> CancelTicketAsync(int eventId, int ticketId, string attendeeId);

        Task<int> TotalAvailablePhysicalTicketsAsync(int eventId);

        Task<int> TotalAvailableVirtualTicketsAsync(int eventId);
    }
}
