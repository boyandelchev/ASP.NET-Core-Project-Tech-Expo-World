namespace TechExpoWorld.Services.Events
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using TechExpoWorld.Data;
    using TechExpoWorld.Data.Models;

    public class EventService : IEventService
    {
        private const string dateFormat = "dd.MM.yyyy";
        private const string physicalTicketType = "Physical";
        private const string virtualTicketType = "Virtual";
        private readonly TechExpoDbContext data;

        public EventService(TechExpoDbContext data)
            => this.data = data;

        public IEnumerable<EventServiceModel> All()
            => this.data
                .Events
                .Select(e => new EventServiceModel
                {
                    Id = e.Id,
                    Title = e.Title,
                    Location = e.Location,
                    StartDate = e.StartDate.ToString(dateFormat, CultureInfo.InvariantCulture),
                    EndDate = e.EndDate.ToString(dateFormat, CultureInfo.InvariantCulture)
                })
                .OrderByDescending(e => e.Id)
                .ToList();

        public EventDetailsServiceModel Details(int eventId)
            => this.data
                .Events
                .Where(e => e.Id == eventId)
                .Select(e => new EventDetailsServiceModel
                {
                    Id = e.Id,
                    Title = e.Title,
                    Content = e.Content,
                    Location = e.Location,
                    StartDate = e.StartDate.ToString(dateFormat, CultureInfo.InvariantCulture),
                    EndDate = e.EndDate.ToString(dateFormat, CultureInfo.InvariantCulture),
                    TotalPhysicalTickets = e.TotalPhysicalTickets,
                    TotalVirtualTickets = e.TotalVirtualTickets,
                    PhysicalTicketPrice = TicketPrice(eventId, physicalTicketType),
                    VirtualTicketPrice = TicketPrice(eventId, virtualTicketType)
                })
                .FirstOrDefault();

        public IEnumerable<MyTicketServiceModel> MyPhysicalTickets(int attendeeId)
            => MyTickets(attendeeId, physicalTicketType);

        public IEnumerable<MyTicketServiceModel> MyVirtualTickets(int attendeeId)
            => MyTickets(attendeeId, virtualTicketType);

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
            var (isStartDate, dateStart) = ValidDate(startDate);
            var (isEndDate, dateEnd) = ValidDate(endDate);

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

            var (isStartDate, dateStart) = ValidDate(startDate);
            var (isEndDate, dateEnd) = ValidDate(endDate);

            if (!isStartDate || !isEndDate)
            {
                return false;
            }

            eventData.StartDate = dateStart;
            eventData.EndDate = dateEnd;

            if (eventData.TotalPhysicalTickets == totalPhysicalTickets &&
                eventData.TotalVirtualTickets == totalVirtualTickets &&
                eventData.Tickets.Any(t => t.Type == physicalTicketType && t.Price == physicalTicketPrice) &&
                eventData.Tickets.Any(t => t.Type == virtualTicketType && t.Price == virtualTicketPrice))
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
            => BuyTicket(eventId, attendeeId, physicalTicketType);

        public bool BuyVirtualTicket(int eventId, int attendeeId)
            => BuyTicket(eventId, attendeeId, virtualTicketType);

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

        public bool IsValidDate(string date)
        {
            var isDate = DateTime.TryParseExact(
                date,
                dateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var dateTime);

            return isDate;
        }

        public bool EventExists(int eventId)
            => this.data.Events.Any(e => e.Id == eventId);

        public int TotalAvailablePhysicalTicketsForEvent(int eventId)
            => TotalAvailableOfTypeTicketsForEvent(eventId, physicalTicketType);

        public int TotalAvailableVirtualTicketsForEvent(int eventId)
            => TotalAvailableOfTypeTicketsForEvent(eventId, virtualTicketType);

        private static (bool, DateTime) ValidDate(string date)
        {
            var isDate = DateTime.TryParseExact(
                date,
                dateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var dateTime);

            return (isDate, dateTime);
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
                    Type = physicalTicketType,
                    Price = physicalTicketPrice
                });
            }

            for (int i = 0; i < totalVirtualTickets; i++)
            {
                tickets.Add(new Ticket
                {
                    Type = virtualTicketType,
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
                .SelectMany(a => a.Tickets
                    .Where(t => t.Type == ticketType)
                    .Select(t => new MyTicketServiceModel
                    {
                        EventId = t.EventId,
                        EventTitle = t.Event.Title,
                        TicketId = t.Id
                    }))
                    .ToList();
    }
}
