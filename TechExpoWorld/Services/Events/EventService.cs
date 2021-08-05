namespace TechExpoWorld.Services.Events
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
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
            var dateStart = ValidDate(startDate);
            var dateEnd = ValidDate(endDate);

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

        private static DateTime ValidDate(string date)
        {
            var isDate = DateTime.TryParseExact(
                date,
                dateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var dateTime);

            return dateTime;
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
    }
}
