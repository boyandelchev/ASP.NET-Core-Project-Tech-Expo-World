namespace TechExpoWorld.Models.Events
{
    using System.Collections.Generic;
    using TechExpoWorld.Services.Events.Models;

    public class MyTicketsViewModel
    {
        public IEnumerable<MyTicketServiceModel> MyPhysicalTickets { get; init; }

        public IEnumerable<MyTicketServiceModel> MyVirtualTickets { get; init; }
    }
}
