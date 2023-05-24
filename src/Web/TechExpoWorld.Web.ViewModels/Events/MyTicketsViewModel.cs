namespace TechExpoWorld.Web.ViewModels.Events
{
    using System.Collections.Generic;

    public class MyTicketsViewModel
    {
        public IEnumerable<MyTicketViewModel> MyPhysicalTickets { get; init; }

        public IEnumerable<MyTicketViewModel> MyVirtualTickets { get; init; }
    }
}
