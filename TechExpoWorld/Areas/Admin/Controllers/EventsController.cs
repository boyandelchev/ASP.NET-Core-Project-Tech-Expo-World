namespace TechExpoWorld.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using TechExpoWorld.Areas.Admin.Models.Events;
    using TechExpoWorld.Infrastructure;
    using TechExpoWorld.Services.Events;

    public class EventsController : AdminController
    {
        private readonly IEventService events;

        public EventsController(IEventService events)
            => this.events = events;

        public IActionResult Add()
        {
            return View(new EventFormModel());
        }

        [HttpPost]
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

            return RedirectToAction(nameof(TechExpoWorld.Controllers.EventsController.All), "Events", new { area = "" });
        }

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

            return RedirectToAction(nameof(TechExpoWorld.Controllers.EventsController.All), "Events", new { area = "" });
        }

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
        public IActionResult Delete(int id)
        {
            var eventExists = this.events.EventExists(id);

            if (!eventExists)
            {
                return NotFound();
            }

            this.events.Delete(id);

            return RedirectToAction(nameof(TechExpoWorld.Controllers.EventsController.All), "Events", new { area = "" });
        }
    }
}
