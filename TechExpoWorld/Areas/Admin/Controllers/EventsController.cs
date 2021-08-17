namespace TechExpoWorld.Areas.Admin.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using TechExpoWorld.Areas.Admin.Models.Events;
    using TechExpoWorld.Infrastructure;
    using TechExpoWorld.Services.Events;

    using static WebConstants;

    public class EventsController : AdminController
    {
        private readonly IEventService events;
        private readonly IMapper mapper;

        public EventsController(IEventService events, IMapper mapper)
        {
            this.events = events;
            this.mapper = mapper;
        }

        public IActionResult Add()
        {
            return View(new EventFormModel());
        }

        [HttpPost]
        public IActionResult Add(EventFormModel eventData)
        {
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

            TempData[GlobalMessageKey] = "Your event was added successfully!";

            return RedirectToAction(nameof(TechExpoWorld.Controllers.EventsController.All),
                                    "Events",
                                    new { area = "" });
        }

        public IActionResult Edit(int id)
        {
            var eventData = this.events.Details(id);

            if (eventData == null)
            {
                return NotFound();
            }

            var eventForm = this.mapper.Map<EventFormModel>(eventData);

            return View(eventForm);
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

            TempData[GlobalMessageKey] = "Your event was edited successfully!";

            return RedirectToAction(nameof(TechExpoWorld.Controllers.EventsController.All),
                                    "Events",
                                    new { area = "" });
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

            TempData[GlobalMessageKey] = "Your event was deleted successfully!";

            return RedirectToAction(nameof(TechExpoWorld.Controllers.EventsController.All),
                                    "Events",
                                    new { area = "" });
        }
    }
}
