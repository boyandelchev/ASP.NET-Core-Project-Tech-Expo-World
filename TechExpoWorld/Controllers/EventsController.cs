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
        private readonly IEventService events;
        private readonly IAttendeeService attendees;

        public EventsController(IEventService events, IAttendeeService attendees)
        {
            this.events = events;
            this.attendees = attendees;
        }

        public async Task<IActionResult> All()
        {
            var eventsAll = await this.events.All();

            return View(eventsAll);
        }

        public async Task<IActionResult> Details(int id, string information)
        {
            var eventData = await this.events.Details(id);

            if (eventData == null)
            {
                return NotFound();
            }

            if (information != eventData.GetEventInformation())
            {
                return BadRequest();
            }

            var totalAvailablePhysicalTickets = await this.events.TotalAvailablePhysicalTickets(id);
            var totalAvailableVirtualTickets = await this.events.TotalAvailableVirtualTickets(id);

            return View(new EventDetailsViewModel
            {
                EventDetails = eventData,
                TotalAvailablePhysicalTickets = totalAvailablePhysicalTickets,
                TotalAvailableVirtualTickets = totalAvailableVirtualTickets
            });
        }

        [Authorize]
        [TypeFilter(typeof(IsAttendeeFilter))]
        public async Task<IActionResult> MyTickets()
        {
            string attendeeId;
            attendeeId = this.HttpContext.Items[nameof(attendeeId)] as string;

            return View(new MyTicketsViewModel
            {
                MyPhysicalTickets = await this.events.MyPhysicalTickets(attendeeId),
                MyVirtualTickets = await this.events.MyVirtualTickets(attendeeId)
            });
        }

        [Authorize]
        [TypeFilter(typeof(IsAttendeeFilter))]
        public async Task<IActionResult> BookPhysicalTicket(int id)
        {
            string attendeeId;
            attendeeId = this.HttpContext.Items[nameof(attendeeId)] as string;

            await this.events.BookPhysicalTicket(id, attendeeId);

            TempData[GlobalMessageKey] = BookedTicket;

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        [TypeFilter(typeof(IsAttendeeFilter))]
        public async Task<IActionResult> BookVirtualTicket(int id)
        {
            string attendeeId;
            attendeeId = this.HttpContext.Items[nameof(attendeeId)] as string;

            await this.events.BookVirtualTicket(id, attendeeId);

            TempData[GlobalMessageKey] = BookedTicket;

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        [TypeFilter(typeof(IsAttendeeFilter))]
        public async Task<IActionResult> CancelTicket(int id, int ticketId)
        {
            string attendeeId;
            attendeeId = this.HttpContext.Items[nameof(attendeeId)] as string;

            var isCancelled = await this.events.CancelTicket(id, ticketId, attendeeId);

            if (!isCancelled)
            {
                return BadRequest();
            }

            TempData[GlobalMessageKey] = CancelledTicket;

            return RedirectToAction(nameof(MyTickets));
        }
    }
}
