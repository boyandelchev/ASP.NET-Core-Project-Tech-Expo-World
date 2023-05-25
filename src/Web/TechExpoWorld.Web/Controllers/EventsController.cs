namespace TechExpoWorld.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using TechExpoWorld.Services.Data.Events;
    using TechExpoWorld.Web.Infrastructure.ActionFilters;
    using TechExpoWorld.Web.Infrastructure.Extensions;
    using TechExpoWorld.Web.ViewModels.Events;

    using static TechExpoWorld.Common.GlobalConstants.TempData;

    public class EventsController : BaseController
    {
        private readonly IEventsService eventsService;

        public EventsController(IEventsService eventsService)
            => this.eventsService = eventsService;

        public async Task<IActionResult> All()
        {
            var eventsAll = await this.eventsService.AllAsync<EventViewModel>();

            return this.View(eventsAll);
        }

        public async Task<IActionResult> Details(int id, string information)
        {
            var eventData = await this.eventsService.DetailsAsync<EventDetailsViewModel>(id);

            if (eventData == null)
            {
                return this.NotFound();
            }

            if (information != eventData.GetEventInformation())
            {
                return this.BadRequest();
            }

            eventData.TotalAvailablePhysicalTickets = await this.eventsService.TotalAvailablePhysicalTicketsAsync(id);
            eventData.TotalAvailableVirtualTickets = await this.eventsService.TotalAvailableVirtualTicketsAsync(id);
            eventData.PhysicalTicketPrice = await this.eventsService.PhysicalTicketPriceAsync(id);
            eventData.VirtualTicketPrice = await this.eventsService.VirtualTicketPriceAsync(id);

            return this.View(eventData);
        }

        [Authorize]
        [TypeFilter(typeof(IsAttendeeFilter))]
        public async Task<IActionResult> TicketsMine()
        {
            string attendeeId = this.HttpContext.Items[nameof(attendeeId)] as string;

            return this.View(new TicketsMineViewModel
            {
                PhysicalTicketsMine = await this.eventsService.PhysicalTicketsMineAsync<TicketMineViewModel>(attendeeId),
                VirtualTicketsMine = await this.eventsService.VirtualTicketsMineAsync<TicketMineViewModel>(attendeeId),
            });
        }

        [Authorize]
        [TypeFilter(typeof(IsAttendeeFilter))]
        public async Task<IActionResult> BookPhysicalTicket(int id)
        {
            string attendeeId = this.HttpContext.Items[nameof(attendeeId)] as string;

            await this.eventsService.BookPhysicalTicketAsync(id, attendeeId);

            this.TempData[GlobalMessageKey] = BookedTicket;

            return this.RedirectToAction(nameof(this.All));
        }

        [Authorize]
        [TypeFilter(typeof(IsAttendeeFilter))]
        public async Task<IActionResult> BookVirtualTicket(int id)
        {
            string attendeeId = this.HttpContext.Items[nameof(attendeeId)] as string;

            await this.eventsService.BookVirtualTicketAsync(id, attendeeId);

            this.TempData[GlobalMessageKey] = BookedTicket;

            return this.RedirectToAction(nameof(this.All));
        }

        [Authorize]
        [TypeFilter(typeof(IsAttendeeFilter))]
        public async Task<IActionResult> CancelTicket(int id, int ticketId)
        {
            string attendeeId = this.HttpContext.Items[nameof(attendeeId)] as string;

            var isCancelled = await this.eventsService.CancelTicketAsync(id, ticketId, attendeeId);

            if (!isCancelled)
            {
                return this.BadRequest();
            }

            this.TempData[GlobalMessageKey] = CancelledTicket;

            return this.RedirectToAction(nameof(this.TicketsMine));
        }
    }
}
