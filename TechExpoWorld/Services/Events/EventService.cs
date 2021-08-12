namespace TechExpoWorld.Services.Events
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
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

        public IEnumerable<EventServiceModel> All()
            => this.data
                .Events
                .ProjectTo<EventServiceModel>(this.mapper.ConfigurationProvider)
                .OrderByDescending(e => e.Id)
                .ToList();

        public EventDetailsServiceModel Details(int eventId)
        {
            var eventData = this.data
                .Events
                .Where(e => e.Id == eventId)
                .ProjectTo<EventDetailsServiceModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefault();

            if (eventData == null)
            {
                return null;
            }

            eventData.PhysicalTicketPrice = TicketPrice(eventId, PhysicalTicketType);
            eventData.VirtualTicketPrice = TicketPrice(eventId, VirtualTicketType);

            return eventData;
        }

        public IEnumerable<MyTicketServiceModel> MyPhysicalTickets(int attendeeId)
            => MyTickets(attendeeId, PhysicalTicketType);

        public IEnumerable<MyTicketServiceModel> MyVirtualTickets(int attendeeId)
            => MyTickets(attendeeId, VirtualTicketType);

        public int CreateEventWithTickets(
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

            var tickets = CreateTickets(
                totalPhysicalTickets,
                physicalTicketPrice,
                totalVirtualTickets,
                virtualTicketPrice);

            eventData.Tickets = tickets;

            this.data.Events.Add(eventData);
            this.data.SaveChanges();

            return eventData.Id;
        }

        public bool Edit(
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
            var eventData = this.data
                .Events
                .Include(e => e.Tickets)
                .Include(e => e.EventAttendees)
                .FirstOrDefault(e => e.Id == eventId);

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
                this.data.SaveChanges();

                return true;
            }
            else
            {
                eventData.EventAttendees = null;
            }

            eventData.TotalPhysicalTickets = totalPhysicalTickets;
            eventData.TotalVirtualTickets = totalVirtualTickets;

            eventData.Tickets = CreateTickets(
                totalPhysicalTickets,
                physicalTicketPrice,
                totalVirtualTickets,
                virtualTicketPrice);

            this.data.SaveChanges();

            return true;
        }

        public bool Delete(int eventId)
        {
            var eventData = this.data
                .Events
                .Include(e => e.EventAttendees)
                .FirstOrDefault(e => e.Id == eventId);

            if (eventData == null)
            {
                return false;
            }

            eventData.EventAttendees = null;

            this.data.Events.Remove(eventData);
            this.data.SaveChanges();

            return true;
        }

        public bool BuyPhysicalTicket(int eventId, int attendeeId)
            => BuyTicket(eventId, attendeeId, PhysicalTicketType);

        public bool BuyVirtualTicket(int eventId, int attendeeId)
            => BuyTicket(eventId, attendeeId, VirtualTicketType);

        public bool RevokeTicket(int eventId, int ticketId, int attendeeId)
        {
            var ticket = this.data
                .Tickets
                .Where(t => t.EventId == eventId &&
                            t.Id == ticketId &&
                            t.AttendeeId == attendeeId)
                .FirstOrDefault();

            if (ticket == null)
            {
                return false;
            }

            ticket.IsSold = false;
            ticket.AttendeeId = null;

            this.data.SaveChanges();

            bool ticketExists = TicketExists(eventId, attendeeId);

            if (!ticketExists)
            {
                var eventAttendee = this.data
                    .EventAttendees
                    .Where(ea => ea.EventId == eventId && ea.AttendeeId == attendeeId)
                    .FirstOrDefault();

                if (eventAttendee != null)
                {
                    this.data.EventAttendees.Remove(eventAttendee);
                    this.data.SaveChanges();
                }
            }

            return true;
        }

        public bool EventExists(int eventId)
            => this.data.Events.Any(e => e.Id == eventId);

        public int TotalAvailablePhysicalTicketsForEvent(int eventId)
            => TotalAvailableOfTypeTicketsForEvent(eventId, PhysicalTicketType);

        public int TotalAvailableVirtualTicketsForEvent(int eventId)
            => TotalAvailableOfTypeTicketsForEvent(eventId, VirtualTicketType);

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

        private static IEnumerable<Ticket> CreateTickets(
            int totalPhysicalTickets,
            decimal physicalTicketPrice,
            int totalVirtualTickets,
            decimal virtualTicketPrice)
        {
            var tickets = new List<Ticket>();

            for (int i = 0; i < totalPhysicalTickets; i++)
            {
                tickets.Add(new Ticket
                {
                    Type = PhysicalTicketType,
                    Price = physicalTicketPrice
                });
            }

            for (int i = 0; i < totalVirtualTickets; i++)
            {
                tickets.Add(new Ticket
                {
                    Type = VirtualTicketType,
                    Price = virtualTicketPrice
                });
            }

            return tickets;
        }

        private decimal TicketPrice(int eventId, string ticketType)
            => this.data
                .Tickets
                .Where(t => t.EventId == eventId && t.Type == ticketType)
                .Select(t => t.Price)
                .FirstOrDefault();

        private int TotalAvailableOfTypeTicketsForEvent(int eventId, string ticketType)
            => this.data
                .Tickets
                .Where(t => t.EventId == eventId &&
                            t.IsSold == false &&
                            t.Type == ticketType)
                .Count();

        private bool BuyTicket(int eventId, int attendeeId, string ticketType)
        {
            var ticket = this.data
                .Tickets
                .Where(t => t.EventId == eventId &&
                            t.IsSold == false &&
                            t.Type == ticketType)
                .FirstOrDefault();

            if (ticket == null)
            {
                return false;
            }

            ticket.IsSold = true;
            ticket.AttendeeId = attendeeId;

            bool eventAttendeeExists = EventAttendeeExists(eventId, attendeeId);

            if (!eventAttendeeExists)
            {
                var eventAttendee = new EventAttendee
                {
                    EventId = eventId,
                    AttendeeId = attendeeId
                };

                this.data.EventAttendees.Add(eventAttendee);
            }

            this.data.SaveChanges();

            return true;
        }

        private bool EventAttendeeExists(int eventId, int attendeeId)
            => this.data
                .EventAttendees
                .Any(ea => ea.EventId == eventId && ea.AttendeeId == attendeeId);

        private bool TicketExists(int eventId, int attendeeId)
            => this.data
                .Tickets
                .Any(t => t.EventId == eventId && t.AttendeeId == attendeeId);

        private IEnumerable<MyTicketServiceModel> MyTickets(int attendeeId, string ticketType)
            => this.data
                .Attendees
                .Where(a => a.Id == attendeeId)
                .SelectMany(a => a.Tickets)
                .Where(t => t.Type == ticketType)
                .ProjectTo<MyTicketServiceModel>(this.mapper.ConfigurationProvider)
                .ToList();
    }
}
