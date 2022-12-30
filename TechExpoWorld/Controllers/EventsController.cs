namespace TechExpoWorld.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> MyTickets()
        {
            if (this.User.IsAdmin())
            {
                return BadRequest();
            }

            var attendeeId = await this.attendees.AttendeeId(this.User.Id());

            if (attendeeId == 0)
            {
                return RedirectToAction(nameof(AttendeesController.BecomeAttendee), ControllerAttendees);
            }

            return View(new MyTicketsViewModel
            {
                MyPhysicalTickets = await this.events.MyPhysicalTickets(attendeeId),
                MyVirtualTickets = await this.events.MyVirtualTickets(attendeeId)
            });
        }

        [Authorize]
        public async Task<IActionResult> BookPhysicalTicket(int id)
        {
            if (this.User.IsAdmin())
            {
                return BadRequest();
            }

            var attendeeId = await this.attendees.AttendeeId(this.User.Id());

            if (attendeeId == 0)
            {
                return RedirectToAction(nameof(AttendeesController.BecomeAttendee), ControllerAttendees);
            }

            await this.events.BookPhysicalTicket(id, attendeeId);

            TempData[GlobalMessageKey] = BookedTicket;

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public async Task<IActionResult> BookVirtualTicket(int id)
        {
            if (this.User.IsAdmin())
            {
                return BadRequest();
            }

            var attendeeId = await this.attendees.AttendeeId(this.User.Id());

            if (attendeeId == 0)
            {
                return RedirectToAction(nameof(AttendeesController.BecomeAttendee), ControllerAttendees);
            }

            await this.events.BookVirtualTicket(id, attendeeId);

            TempData[GlobalMessageKey] = BookedTicket;

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public async Task<IActionResult> RevokeTicket(int id, int ticketId)
        {
            if (this.User.IsAdmin())
            {
                return BadRequest();
            }

            var attendeeId = await this.attendees.AttendeeId(this.User.Id());

            if (attendeeId == 0)
            {
                return RedirectToAction(nameof(AttendeesController.BecomeAttendee), ControllerAttendees);
            }

            var isRevoked = await this.events.RevokeTicket(id, ticketId, attendeeId);

            if (!isRevoked)
            {
                return BadRequest();
            }

            TempData[GlobalMessageKey] = RevokedTicket;

            return RedirectToAction(nameof(MyTickets));
        }
    }
}
