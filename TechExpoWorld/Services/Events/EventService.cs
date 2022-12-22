namespace TechExpoWorld.Services.Events
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using TechExpoWorld.Data;
    using TechExpoWorld.Data.Models;
    using TechExpoWorld.Services.Events.Models;

    using static Data.DataConstants.Event;

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
                .ProjectTo<EventServiceModel>(this.mapper.ConfigurationProvider)
                .OrderByDescending(e => e.Id)
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

        public async Task<IEnumerable<MyTicketServiceModel>> MyPhysicalTickets(int attendeeId)
            => await MyTickets(attendeeId, PhysicalTicketType);

        public async Task<IEnumerable<MyTicketServiceModel>> MyVirtualTickets(int attendeeId)
            => await MyTickets(attendeeId, VirtualTicketType);

        public async Task<int> CreateEventWithTickets(
            string title,
            string content,
            string location,
            string startDate,
            string endDate,
            int totalPhysicalTickets,
            decimal physicalTicketPrice,
            int totalVirtualTickets,
            decimal virtualTicketPrice,
            string userId)
        {
            var (isStartDate, dateStart) = ValidDate(startDate, DateFormatOne, DateFormatTwo, DateFormatThree);
            var (isEndDate, dateEnd) = ValidDate(endDate, DateFormatOne, DateFormatTwo, DateFormatThree);

            var eventData = new Event
            {
                Title = title,
                Content = content,
                Location = location,
                StartDate = dateStart,
                EndDate = dateEnd,
                TotalPhysicalTickets = totalPhysicalTickets,
                TotalVirtualTickets = totalVirtualTickets,
                UserId = userId
            };

            var tickets = CreateAllTickets(
                totalPhysicalTickets,
                physicalTicketPrice,
                totalVirtualTickets,
                virtualTicketPrice);

            eventData.Tickets = tickets;

            await this.data.Events.AddAsync(eventData);
            await this.data.SaveChangesAsync();

            return eventData.Id;
        }

        public async Task<bool> Edit(
            int eventId,
            string title,
            string content,
            string location,
            string startDate,
            string endDate,
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

            var (isStartDate, dateStart) = ValidDate(startDate, DateFormatOne, DateFormatTwo, DateFormatThree);
            var (isEndDate, dateEnd) = ValidDate(endDate, DateFormatOne, DateFormatTwo, DateFormatThree);

            eventData.StartDate = dateStart;
            eventData.EndDate = dateEnd;

            if (eventData.TotalPhysicalTickets == totalPhysicalTickets &&
                eventData.TotalVirtualTickets == totalVirtualTickets &&
                eventData.Tickets.Any(t => t.Type == PhysicalTicketType && t.Price == physicalTicketPrice) &&
                eventData.Tickets.Any(t => t.Type == VirtualTicketType && t.Price == virtualTicketPrice))
            {
                await this.data.SaveChangesAsync();

                return true;
            }

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
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (eventData == null)
            {
                return false;
            }

            this.data.Events.Remove(eventData);
            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<bool> BuyPhysicalTicket(int eventId, int attendeeId)
            => await BuyTicket(eventId, attendeeId, PhysicalTicketType);

        public async Task<bool> BuyVirtualTicket(int eventId, int attendeeId)
            => await BuyTicket(eventId, attendeeId, VirtualTicketType);

        public async Task<bool> RevokeTicket(int eventId, int ticketId, int attendeeId)
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

            ticket.IsSold = false;
            ticket.AttendeeId = null;

            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EventExists(int eventId)
            => await this.data
                .Events
                .AnyAsync(e => e.Id == eventId);

        public async Task<int> TotalAvailablePhysicalTicketsForEvent(int eventId)
            => await TotalAvailableOfTypeTicketsForEvent(eventId, PhysicalTicketType);

        public async Task<int> TotalAvailableVirtualTicketsForEvent(int eventId)
            => await TotalAvailableOfTypeTicketsForEvent(eventId, VirtualTicketType);

        private static (bool, DateTime) ValidDate(string date, string dateFormatOne, string dateFormatTwo, string dateFormatThree)
        {
            var isDate = DateTime.TryParseExact(
                date,
                dateFormatOne,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var dateTimeOne);

            if (isDate)
            {
                return (isDate, dateTimeOne);
            }

            isDate = DateTime.TryParseExact(
                date,
                dateFormatTwo,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var dateTimeTwo);

            if (isDate)
            {
                return (isDate, dateTimeTwo);
            }

            isDate = DateTime.TryParseExact(
                date,
                dateFormatThree,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var dateTimeThree);

            return (isDate, dateTimeThree);
        }

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

        private async Task<decimal> TicketPrice(int eventId, string ticketType)
            => await this.data
                .Tickets
                .Where(t => t.EventId == eventId && t.Type == ticketType)
                .Select(t => t.Price)
                .FirstOrDefaultAsync();

        private async Task<int> TotalAvailableOfTypeTicketsForEvent(int eventId, string ticketType)
            => await this.data
                .Tickets
                .Where(t => t.EventId == eventId &&
                            t.IsSold == false &&
                            t.Type == ticketType)
                .CountAsync();

        private async Task<bool> BuyTicket(int eventId, int attendeeId, string ticketType)
        {
            var ticket = await this.data
                .Tickets
                .Where(t => t.EventId == eventId &&
                            t.IsSold == false &&
                            t.Type == ticketType)
                .FirstOrDefaultAsync();

            if (ticket == null)
            {
                return false;
            }

            ticket.IsSold = true;
            ticket.AttendeeId = attendeeId;

            await this.data.SaveChangesAsync();

            return true;
        }

        private async Task<IEnumerable<MyTicketServiceModel>> MyTickets(int attendeeId, string ticketType)
            => await this.data
                .Attendees
                .Where(a => a.Id == attendeeId)
                .SelectMany(a => a.Tickets)
                .Where(t => t.Type == ticketType)
                .ProjectTo<MyTicketServiceModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();
    }
}
