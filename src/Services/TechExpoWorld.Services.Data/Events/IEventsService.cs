namespace TechExpoWorld.Services.Data.Events
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IEventsService
    {
        Task<IEnumerable<T>> AllAsync<T>();

        Task<T> DetailsAsync<T>(int eventId);

        Task<IEnumerable<T>> PhysicalTicketsMineAsync<T>(string attendeeId);

        Task<IEnumerable<T>> VirtualTicketsMineAsync<T>(string attendeeId);

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

        Task<decimal> PhysicalTicketPriceAsync(int eventId);

        Task<decimal> VirtualTicketPriceAsync(int eventId);
    }
}
