namespace TechExpoWorld.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TechExpoWorld.Infrastructure.Extensions;
    using TechExpoWorld.Models.Events;
    using TechExpoWorld.Services.Attendees;
    using TechExpoWorld.Services.Events;

    using static WebConstants;

    public class EventsController : Controller
    {
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

            var totalAvailablePhysicalTicketsForEvent = await this.events.TotalAvailablePhysicalTicketsForEvent(id);
            var totalAvailableVirtualTicketsForEvent = await this.events.TotalAvailableVirtualTicketsForEvent(id);

            return View(new EventDetailsViewModel
            {
                EventDetails = eventData,
                TotalAvailablePhysicalTicketsForEvent = totalAvailablePhysicalTicketsForEvent,
                TotalAvailableVirtualTicketsForEvent = totalAvailableVirtualTicketsForEvent
            });
        }

        [Authorize]
        public async Task<IActionResult> MyTickets()
        {
            var userId = this.User.Id();
            var attendeeId = await this.attendees.AttendeeId(userId);

            if (attendeeId == 0 && !this.User.IsAdmin())
            {
                return RedirectToAction(nameof(AttendeesController.BecomeAttendee), "Attendees");
            }

            return View(new MyTicketsViewModel
            {
                MyPhysicalTickets = await this.events.MyPhysicalTickets(attendeeId),
                MyVirtualTickets = await this.events.MyVirtualTickets(attendeeId)
            });
        }

        [Authorize]
        public async Task<IActionResult> BuyPhysicalTicket(int id)
        {
            if (!await this.events.EventExists(id))
            {
                return NotFound();
            }

            if (this.User.IsAdmin())
            {
                return BadRequest();
            }

            var userId = this.User.Id();
            var attendeeId = await this.attendees.AttendeeId(userId);

            if (attendeeId == 0)
            {
                return RedirectToAction(nameof(AttendeesController.BecomeAttendee), "Attendees");
            }

            await this.events.BuyPhysicalTicket(id, attendeeId);

            TempData[GlobalMessageKey] = "Your have booked a ticket successfully!";

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public async Task<IActionResult> BuyVirtualTicket(int id)
        {
            if (!await this.events.EventExists(id))
            {
                return NotFound();
            }

            if (this.User.IsAdmin())
            {
                return BadRequest();
            }

            var userId = this.User.Id();
            var attendeeId = await this.attendees.AttendeeId(userId);

            if (attendeeId == 0)
            {
                return RedirectToAction(nameof(AttendeesController.BecomeAttendee), "Attendees");
            }

            await this.events.BuyVirtualTicket(id, attendeeId);

            TempData[GlobalMessageKey] = "Your have booked a ticket successfully!";

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public async Task<IActionResult> RevokeTicket(int id, int ticketId)
        {
            var userId = this.User.Id();
            var attendeeId = await this.attendees.AttendeeId(userId);

            if (attendeeId == 0 && !this.User.IsAdmin())
            {
                return RedirectToAction(nameof(AttendeesController.BecomeAttendee), "Attendees");
            }

            var isRevoked = await this.events.RevokeTicket(id, ticketId, attendeeId);

            if (!isRevoked)
            {
                return BadRequest();
            }

            TempData[GlobalMessageKey] = "Your have revoked a ticket successfully!";

            return RedirectToAction(nameof(MyTickets));
        }
    }
}
