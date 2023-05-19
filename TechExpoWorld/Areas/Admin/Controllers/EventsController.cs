namespace TechExpoWorld.Areas.Admin.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Mvc;

    using TechExpoWorld.Infrastructure.Extensions;
    using TechExpoWorld.Models.Events;
    using TechExpoWorld.Services.Events;

    using static GlobalConstants.TempData;

    public class EventsController : AdminController
    {
        private const string ControllerEvents = "Events";
        private readonly IEventsService events;
        private readonly IMapper mapper;

        public EventsController(IEventsService events, IMapper mapper)
        {
            this.events = events;
            this.mapper = mapper;
        }

        public IActionResult Add() => View();

        [HttpPost]
        public async Task<IActionResult> Add(EventFormModel eventData)
        {
            if (!ModelState.IsValid)
            {
                return View(eventData);
            }

            var userId = this.User.Id();

            await this.events.CreateEventWithTicketsAsync(
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

            TempData[GlobalMessageKey] = CreatedEvent;

            return RedirectToAction(
                nameof(TechExpoWorld.Controllers.EventsController.All),
                ControllerEvents,
                new { area = string.Empty });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var eventData = await this.events.DetailsAsync(id);

            if (eventData == null)
            {
                return NotFound();
            }

            var eventForm = this.mapper.Map<EventFormModel>(eventData);

            return View(eventForm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EventFormModel eventData)
        {
            if (!ModelState.IsValid)
            {
                return View(eventData);
            }

            await this.events.EditAsync(
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

            TempData[GlobalMessageKey] = EditedEvent;

            return RedirectToAction(
                nameof(TechExpoWorld.Controllers.EventsController.All),
                ControllerEvents,
                new { area = string.Empty });
        }

        public async Task<IActionResult> Delete(int id) => await Edit(id);

        [ActionName(nameof(Delete))]
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await this.events.DeleteAsync(id);

            TempData[GlobalMessageKey] = DeletedEvent;

            return RedirectToAction(
                nameof(TechExpoWorld.Controllers.EventsController.All),
                ControllerEvents,
                new { area = string.Empty });
        }
    }
}
