namespace TechExpoWorld.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TechExpoWorld.Infrastructure;
    using TechExpoWorld.Models.Events;
    using TechExpoWorld.Services.Events;

    using static WebConstants;

    public class EventsController : Controller
    {
        private readonly IEventService events;

        public EventsController(IEventService events)
            => this.events = events;

        public IActionResult All()
        {
            var eventsAll = this.events.All();

            return View(eventsAll);
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
