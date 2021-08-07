namespace TechExpoWorld.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TechExpoWorld.Infrastructure;
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

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Add()
        {
            return View(new EventFormModel());
        }

        [HttpPost]
        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Add(EventFormModel eventData)
        {
            if (!this.events.IsValidDate(eventData.StartDate))
            {
                this.ModelState.AddModelError(nameof(eventData.StartDate), "The field Start Date must be in format '15.08.2021'!");
            }

            if (!this.events.IsValidDate(eventData.EndDate))
            {
                this.ModelState.AddModelError(nameof(eventData.EndDate), "The field End Date must be in format '15.08.2021'!");
            }

            if (!ModelState.IsValid)
            {
                return View(eventData);
            }

            var userId = this.User.Id();

            this.events.CreateEventWithTickets(
                eventData.Title,
                eventData.Content,
                eventData.Location,
                eventData.StartDate,
                eventData.EndDate,
                eventData.TotalPhysicalTickets,
                eventData.PhysicalTicketPrice,
                eventData.TotalVirtualTickets,
                eventData.VirtualTicketPrice,
                userId);

            return RedirectToAction(nameof(All));
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Edit(int id)
        {
            var eventData = this.events.Details(id);

            if (eventData == null)
            {
                return NotFound();
            }

            return View(new EventFormModel
            {
                Title = eventData.Title,
                Content = eventData.Content,
                Location = eventData.Location,
                StartDate = eventData.StartDate,
                EndDate = eventData.EndDate,
                TotalPhysicalTickets = eventData.TotalPhysicalTickets,
                TotalVirtualTickets = eventData.TotalVirtualTickets,
                PhysicalTicketPrice = eventData.PhysicalTicketPrice,
                VirtualTicketPrice = eventData.VirtualTicketPrice
            });
        }

        [HttpPost]
        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Edit(int id, EventFormModel eventData)
        {
            var eventExists = this.events.EventExists(id);

            if (!eventExists)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(eventData);
            }

            this.events.Edit(
                id,
                eventData.Title,
                eventData.Content,
                eventData.Location,
                eventData.StartDate,
                eventData.EndDate,
                eventData.TotalPhysicalTickets,
                eventData.PhysicalTicketPrice,
                eventData.TotalVirtualTickets,
                eventData.VirtualTicketPrice);

            return RedirectToAction(nameof(All));
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult DeleteDetails(int id)
        {
            var eventData = this.events.Details(id);

            if (eventData == null)
            {
                return NotFound();
            }

            return View(eventData);
        }

        [HttpPost]
        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Delete(int id)
        {
            var eventExists = this.events.EventExists(id);

            if (!eventExists)
            {
                return NotFound();
            }

            this.events.Delete(id);

            return RedirectToAction(nameof(All));
        }
    }
}
