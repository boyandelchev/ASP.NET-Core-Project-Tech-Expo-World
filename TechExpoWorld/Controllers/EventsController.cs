namespace TechExpoWorld.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TechExpoWorld.Infrastructure;
    using TechExpoWorld.Models.Events;
    using TechExpoWorld.Services.Attendees;
    using TechExpoWorld.Services.Events;

    public class EventsController : Controller
    {
        private readonly IEventService events;
        private readonly IAttendeeService attendees;

        public EventsController(IEventService events, IAttendeeService attendees)
        {
            this.events = events;
            this.attendees = attendees;
        }

        public IActionResult All()
        {
            var eventsAll = this.events.All();

            return View(eventsAll);
        }

        public IActionResult Details(int id)
        {
            var eventData = this.events.Details(id);

            if (eventData == null)
            {
                return NotFound();
            }

            var totalAvailablePhysicalTicketsForEvent = this.events.TotalAvailablePhysicalTicketsForEvent(id);
            var totalAvailableVirtualTicketsForEvent = this.events.TotalAvailableVirtualTicketsForEvent(id);

            return View(new EventDetailsViewModel
            {
                EventDetails = eventData,
                TotalAvailablePhysicalTicketsForEvent = totalAvailablePhysicalTicketsForEvent,
                TotalAvailableVirtualTicketsForEvent = totalAvailableVirtualTicketsForEvent
            });
        }

        [Authorize]
        public IActionResult MyTickets()
        {
            var userId = this.User.Id();
            var attendeeId = this.attendees.AttendeeId(userId);

            if (attendeeId == 0 && !this.User.IsAdmin())
            {
                return RedirectToAction(nameof(AttendeesController.BecomeAttendee), "Attendees");
            }

            return View(new MyTicketsViewModel
            {
                MyPhysicalTickets = this.events.MyPhysicalTickets(attendeeId),
                MyVirtualTickets = this.events.MyVirtualTickets(attendeeId)
            });
        }

        [Authorize]
        public IActionResult BuyPhysicalTicket(int id)
        {
            if (!this.events.EventExists(id))
            {
                return NotFound();
            }

            if (this.User.IsAdmin())
            {
                return BadRequest();
            }

            var userId = this.User.Id();
            var attendeeId = this.attendees.AttendeeId(userId);

            if (attendeeId == 0)
            {
                return RedirectToAction(nameof(AttendeesController.BecomeAttendee), "Attendees");
            }

            this.events.BuyPhysicalTicket(id, attendeeId);

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult BuyVirtualTicket(int id)
        {
            if (!this.events.EventExists(id))
            {
                return NotFound();
            }

            if (this.User.IsAdmin())
            {
                return BadRequest();
            }

            var userId = this.User.Id();
            var attendeeId = this.attendees.AttendeeId(userId);

            if (attendeeId == 0)
            {
                return RedirectToAction(nameof(AttendeesController.BecomeAttendee), "Attendees");
            }

            this.events.BuyVirtualTicket(id, attendeeId);

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult RevokeTicket(int id, int ticketId)
        {
            var userId = this.User.Id();
            var attendeeId = this.attendees.AttendeeId(userId);

            if (attendeeId == 0 && !this.User.IsAdmin())
            {
                return RedirectToAction(nameof(AttendeesController.BecomeAttendee), "Attendees");
            }

            var isRevoked = this.events.RevokeTicket(id, ticketId, attendeeId);

            if (!isRevoked)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(MyTickets));
        }
    }
}
