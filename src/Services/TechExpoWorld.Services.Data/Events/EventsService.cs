namespace TechExpoWorld.Services.Data.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using TechExpoWorld.Data.Common.Repositories;
    using TechExpoWorld.Data.Models;
    using TechExpoWorld.Services.Mapping;

    public class EventsService : IEventsService
    {
        private const string PhysicalTicketType = "Physical";
        private const string VirtualTicketType = "Virtual";
        private readonly IDeletableEntityRepository<Event> eventsRepository;
        private readonly IDeletableEntityRepository<Ticket> ticketsRepository;
        private readonly IDeletableEntityRepository<Attendee> attendeesRepository;

        public EventsService(
            IDeletableEntityRepository<Event> eventsRepository,
            IDeletableEntityRepository<Ticket> ticketsRepository,
            IDeletableEntityRepository<Attendee> attendeesRepository)
        {
            this.eventsRepository = eventsRepository;
            this.ticketsRepository = ticketsRepository;
            this.attendeesRepository = attendeesRepository;
        }

        public async Task<IEnumerable<T>> AllAsync<T>()
            => await this.eventsRepository
                .All()
                .OrderByDescending(e => e.Id)
                .To<T>()
                .ToListAsync();

        public async Task<T> DetailsAsync<T>(int eventId)
            => await this.eventsRepository
                .All()
                .Where(e => e.Id == eventId)
                .To<T>()
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<T>> PhysicalTicketsMineAsync<T>(string attendeeId)
            => await this.TicketsMineAsync<T>(attendeeId, PhysicalTicketType);

        public async Task<IEnumerable<T>> VirtualTicketsMineAsync<T>(string attendeeId)
            => await this.TicketsMineAsync<T>(attendeeId, VirtualTicketType);

        public async Task<int> CreateEventWithTicketsAsync(
            string title,
            string content,
            string location,
            DateTime? startDate,
            DateTime? endDate,
            int totalPhysicalTickets,
            decimal physicalTicketPrice,
            int totalVirtualTickets,
            decimal virtualTicketPrice,
            string userId)
        {
            var eventData = new Event
            {
                Title = title,
                Content = content,
                Location = location,
                StartDate = startDate.Value,
                EndDate = endDate.Value,
                TotalPhysicalTickets = totalPhysicalTickets,
                TotalVirtualTickets = totalVirtualTickets,
                UserId = userId,
            };

            eventData.Tickets = CreateAllTickets(
                totalPhysicalTickets,
                physicalTicketPrice,
                totalVirtualTickets,
                virtualTicketPrice);

            await this.eventsRepository.AddAsync(eventData);
            await this.eventsRepository.SaveChangesAsync();

            return eventData.Id;
        }

        public async Task<bool> EditAsync(
            int eventId,
            string title,
            string content,
            string location,
            DateTime? startDate,
            DateTime? endDate,
            int totalPhysicalTickets,
            decimal physicalTicketPrice,
            int totalVirtualTickets,
            decimal virtualTicketPrice)
        {
            var eventData = await this.eventsRepository
                .All()
                .Include(e => e.Tickets)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (eventData == null)
            {
                return false;
            }

            eventData.Title = title;
            eventData.Content = content;
            eventData.Location = location;
            eventData.StartDate = startDate.Value;
            eventData.EndDate = endDate.Value;

            if (eventData.TotalPhysicalTickets == totalPhysicalTickets &&
                eventData.TotalVirtualTickets == totalVirtualTickets &&
                eventData.Tickets.Any(t => t.Type == PhysicalTicketType && t.Price == physicalTicketPrice) &&
                eventData.Tickets.Any(t => t.Type == VirtualTicketType && t.Price == virtualTicketPrice))
            {
                await this.eventsRepository.SaveChangesAsync();

                return true;
            }

            RevokeTickets(eventData.Tickets);

            eventData.TotalPhysicalTickets = totalPhysicalTickets;
            eventData.TotalVirtualTickets = totalVirtualTickets;

            eventData.Tickets = CreateAllTickets(
                totalPhysicalTickets,
                physicalTicketPrice,
                totalVirtualTickets,
                virtualTicketPrice);

            await this.eventsRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int eventId)
        {
            var eventData = await this.eventsRepository
                .All()
                .Include(e => e.Tickets)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (eventData == null)
            {
                return false;
            }

            RevokeTickets(eventData.Tickets);

            this.eventsRepository.Delete(eventData);
            await this.eventsRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> BookPhysicalTicketAsync(int eventId, string attendeeId)
            => await this.BookTicketAsync(eventId, attendeeId, PhysicalTicketType);

        public async Task<bool> BookVirtualTicketAsync(int eventId, string attendeeId)
            => await this.BookTicketAsync(eventId, attendeeId, VirtualTicketType);

        public async Task<bool> CancelTicketAsync(int eventId, int ticketId, string attendeeId)
        {
            var ticket = await this.ticketsRepository
                .All()
                .Where(t => t.EventId == eventId &&
                            t.Id == ticketId &&
                            t.AttendeeId == attendeeId)
                .FirstOrDefaultAsync();

            if (ticket == null)
            {
                return false;
            }

            ticket.IsBooked = false;
            ticket.AttendeeId = null;

            await this.ticketsRepository.SaveChangesAsync();

            return true;
        }

        public async Task<int> TotalAvailablePhysicalTicketsAsync(int eventId)
            => await this.TotalAvailableOfTypeTicketsAsync(eventId, PhysicalTicketType);

        public async Task<int> TotalAvailableVirtualTicketsAsync(int eventId)
            => await this.TotalAvailableOfTypeTicketsAsync(eventId, VirtualTicketType);

        public async Task<decimal> PhysicalTicketPriceAsync(int eventId)
            => await this.TicketPriceAsync(eventId, PhysicalTicketType);

        public async Task<decimal> VirtualTicketPriceAsync(int eventId)
            => await this.TicketPriceAsync(eventId, VirtualTicketType);

        private static IEnumerable<Ticket> CreateAllTickets(
            int totalPhysicalTickets,
            decimal physicalTicketPrice,
            int totalVirtualTickets,
            decimal virtualTicketPrice)
        {
            var ticketsAll = new List<Ticket>();

            var physicalTickets = CreateTicketsOfType(
                totalPhysicalTickets,
                PhysicalTicketType,
                physicalTicketPrice);
            ticketsAll.AddRange(physicalTickets);

            var virtualTickets = CreateTicketsOfType(
                totalVirtualTickets,
                VirtualTicketType,
                virtualTicketPrice);
            ticketsAll.AddRange(virtualTickets);

            return ticketsAll;
        }

        private static IEnumerable<Ticket> CreateTicketsOfType(
            int totalTickets,
            string ticketType,
            decimal ticketPrice)
        {
            var tickets = new List<Ticket>();

            for (int i = 0; i < totalTickets; i++)
            {
                tickets.Add(new Ticket
                {
                    Type = ticketType,
                    Price = ticketPrice,
                });
            }

            return tickets;
        }

        private static void RevokeTickets(IEnumerable<Ticket> tickets)
        {
            foreach (var ticket in tickets)
            {
                ticket.IsBooked = false;
                ticket.EventId = null;
                ticket.AttendeeId = null;
            }
        }

        private async Task<decimal> TicketPriceAsync(int eventId, string ticketType)
            => await this.ticketsRepository
                .All()
                .Where(t => t.EventId == eventId && t.Type == ticketType)
                .Select(t => t.Price)
                .FirstOrDefaultAsync();

        private async Task<int> TotalAvailableOfTypeTicketsAsync(int eventId, string ticketType)
            => await this.AvailableOfTypeTicketsQuery(eventId, ticketType)
                .CountAsync();

        private async Task<bool> BookTicketAsync(int eventId, string attendeeId, string ticketType)
        {
            var ticket = await this.AvailableOfTypeTicketsQuery(eventId, ticketType)
                .FirstOrDefaultAsync();

            if (ticket == null)
            {
                return false;
            }

            ticket.IsBooked = true;
            ticket.AttendeeId = attendeeId;

            await this.ticketsRepository.SaveChangesAsync();

            return true;
        }

        private async Task<IEnumerable<T>> TicketsMineAsync<T>(string attendeeId, string ticketType)
            => await this.attendeesRepository
                .All()
                .Where(a => a.Id == attendeeId)
                .SelectMany(a => a.Tickets)
                .Where(t => t.Type == ticketType)
                .To<T>()
                .ToListAsync();

        private IQueryable<Ticket> AvailableOfTypeTicketsQuery(int eventId, string ticketType)
            => this.ticketsRepository
                .All()
                .Where(t => t.EventId == eventId &&
                            t.IsBooked == false &&
                            t.Type == ticketType)
                .AsQueryable();
    }
}
