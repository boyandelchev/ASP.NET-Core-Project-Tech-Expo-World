namespace TechExpoWorld.Services.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using TechExpoWorld.Data;
    using TechExpoWorld.Data.Models;
    using TechExpoWorld.Services.Events.Models;

    public class EventService : IEventService
    {
        private const string PhysicalTicketType = "Physical";
        private const string VirtualTicketType = "Virtual";
        private readonly TechExpoDbContext data;
        private readonly IMapper mapper;

        public EventService(TechExpoDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<EventServiceModel>> All()
            => await this.data
                .Events
                .OrderByDescending(e => e.Id)
                .ProjectTo<EventServiceModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<EventDetailsServiceModel> Details(int eventId)
        {
            var eventData = await this.data
                .Events
                .Where(e => e.Id == eventId)
                .ProjectTo<EventDetailsServiceModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (eventData == null)
            {
                return null;
            }

            eventData.PhysicalTicketPrice = await TicketPrice(eventId, PhysicalTicketType);
            eventData.VirtualTicketPrice = await TicketPrice(eventId, VirtualTicketType);

            return eventData;
        }

        public async Task<IEnumerable<MyTicketServiceModel>> MyPhysicalTickets(string attendeeId)
            => await MyTickets(attendeeId, PhysicalTicketType);

        public async Task<IEnumerable<MyTicketServiceModel>> MyVirtualTickets(string attendeeId)
            => await MyTickets(attendeeId, VirtualTicketType);

        public async Task<int> CreateEventWithTickets(
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
                UserId = userId
            };

            eventData.Tickets = CreateAllTickets(
                totalPhysicalTickets,
                physicalTicketPrice,
                totalVirtualTickets,
                virtualTicketPrice);

            await this.data.Events.AddAsync(eventData);
            await this.data.SaveChangesAsync();

            return eventData.Id;
        }

        public async Task<bool> Edit(
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
            var eventData = await this.data
                .Events
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
                await this.data.SaveChangesAsync();

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

            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(int eventId)
        {
            var eventData = await this.data
                .Events
                .Include(e => e.Tickets)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (eventData == null)
            {
                return false;
            }

            RevokeTickets(eventData.Tickets);

            this.data.Events.Remove(eventData);
            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<bool> BookPhysicalTicket(int eventId, string attendeeId)
            => await BookTicket(eventId, attendeeId, PhysicalTicketType);

        public async Task<bool> BookVirtualTicket(int eventId, string attendeeId)
            => await BookTicket(eventId, attendeeId, VirtualTicketType);

        public async Task<bool> CancelTicket(int eventId, int ticketId, string attendeeId)
        {
            var ticket = await this.data
                .Tickets
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

            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<int> TotalAvailablePhysicalTickets(int eventId)
            => await TotalAvailableOfTypeTickets(eventId, PhysicalTicketType);

        public async Task<int> TotalAvailableVirtualTickets(int eventId)
            => await TotalAvailableOfTypeTickets(eventId, VirtualTicketType);

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
                    Price = ticketPrice
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

        private async Task<decimal> TicketPrice(int eventId, string ticketType)
            => await this.data
                .Tickets
                .Where(t => t.EventId == eventId && t.Type == ticketType)
                .Select(t => t.Price)
                .FirstOrDefaultAsync();

        private async Task<int> TotalAvailableOfTypeTickets(int eventId, string ticketType)
            => await AvailableOfTypeTicketsQuery(eventId, ticketType)
                .CountAsync();

        private async Task<bool> BookTicket(int eventId, string attendeeId, string ticketType)
        {
            var ticket = await AvailableOfTypeTicketsQuery(eventId, ticketType)
                .FirstOrDefaultAsync();

            if (ticket == null)
            {
                return false;
            }

            ticket.IsBooked = true;
            ticket.AttendeeId = attendeeId;

            await this.data.SaveChangesAsync();

            return true;
        }

        private async Task<IEnumerable<MyTicketServiceModel>> MyTickets(string attendeeId, string ticketType)
            => await this.data
                .Attendees
                .Where(a => a.Id == attendeeId)
                .SelectMany(a => a.Tickets)
                .Where(t => t.Type == ticketType)
                .ProjectTo<MyTicketServiceModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        private IQueryable<Ticket> AvailableOfTypeTicketsQuery(int eventId, string ticketType)
            => this.data
                .Tickets
                .Where(t => t.EventId == eventId &&
                            t.IsBooked == false &&
                            t.Type == ticketType)
                .AsQueryable();
    }
}
