namespace TechExpoWorld.Web.ViewModels.Events
{
    using System.Collections.Generic;

    public class TicketsMineViewModel
    {
        public IEnumerable<TicketMineViewModel> PhysicalTicketsMine { get; init; }

        public IEnumerable<TicketMineViewModel> VirtualTicketsMine { get; init; }
    }
}
