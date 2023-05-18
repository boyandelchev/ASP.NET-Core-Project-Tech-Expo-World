namespace TechExpoWorld.Test.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TechExpoWorld.Data.Models;

    public static class Events
    {
        public static IEnumerable<Event> TenEvents
            => Enumerable.Range(0, 10).Select(i => new Event
            {
                Title = "AI"
            });

        public static Event OneEvent
            => new Event
            {
                Id = 1,
                Title = "Event",
                Content = "content",
                Location = "London",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow,
                TotalPhysicalTickets = 10,
                TotalVirtualTickets = 10
            };
    }
}
