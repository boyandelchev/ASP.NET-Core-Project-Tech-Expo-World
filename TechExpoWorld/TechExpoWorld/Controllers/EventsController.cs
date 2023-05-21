namespace TechExpoWorld.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using TechExpoWorld.Infrastructure.ActionFilters;
    using TechExpoWorld.Infrastructure.Extensions;
    using TechExpoWorld.Models.Events;
    using TechExpoWorld.Services.Attendees;
    using TechExpoWorld.Services.Events;

    using static GlobalConstants.TempData;

    public class EventsController : Controller
    {
        private const string ControllerAttendees = "Attendees";
        private readonly IEventsService events;
        private readonly IAttendeesService attendees;

        public EventsController(IEventsService events, IAttendeesService attendees)
        {
            this.events = events;
            this.attendees = attendees;
        }

        public async Task<IActionResult> All()
        {
            var eventsAll = await this.events.AllAsync();

            return View(eventsAll);
        }

        public async Task<IActionResult> Details(int id, string information)
        {
            var eventData = await this.events.DetailsAsync(id);

            if (eventData == null)
            {
                return NotFound();
            }

            if (information != eventData.GetEventInformation())
            {
                return BadRequest();
            }

            return View(new EventDetailsViewModel
            {
                EventDetails = eventData,
                TotalAvailablePhysicalTickets = await this.events.TotalAvailablePhysicalTicketsAsync(id),
                TotalAvailableVirtualTickets = await this.events.TotalAvailableVirtualTicketsAsync(id)
            });
        }

        [Authorize]
        [TypeFilter(typeof(IsAttendeeFilter))]
        public async Task<IActionResult> MyTickets()
        {
            string attendeeId = this.HttpContext.Items[nameof(attendeeId)] as string;

            return View(new MyTicketsViewModel
            {
                MyPhysicalTickets = await this.events.MyPhysicalTicketsAsync(attendeeId),
                MyVirtualTickets = await this.events.MyVirtualTicketsAsync(attendeeId)
            });
        }

        [Authorize]
        [TypeFilter(typeof(IsAttendeeFilter))]
        public async Task<IActionResult> BookPhysicalTicket(int id)
        {
            string attendeeId = this.HttpContext.Items[nameof(attendeeId)] as string;

            await this.events.BookPhysicalTicketAsync(id, attendeeId);

            TempData[GlobalMessageKey] = BookedTicket;

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        [TypeFilter(typeof(IsAttendeeFilter))]
        public async Task<IActionResult> BookVirtualTicket(int id)
        {
            string attendeeId = this.HttpContext.Items[nameof(attendeeId)] as string;

            await this.events.BookVirtualTicketAsync(id, attendeeId);

            TempData[GlobalMessageKey] = BookedTicket;

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        [TypeFilter(typeof(IsAttendeeFilter))]
        public async Task<IActionResult> CancelTicket(int id, int ticketId)
        {
            string attendeeId = this.HttpContext.Items[nameof(attendeeId)] as string;

            var isCancelled = await this.events.CancelTicketAsync(id, ticketId, attendeeId);

            if (!isCancelled)
            {
                return BadRequest();
            }

            TempData[GlobalMessageKey] = CancelledTicket;

            return RedirectToAction(nameof(MyTickets));
        }
    }
}
