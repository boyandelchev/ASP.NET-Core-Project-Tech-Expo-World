﻿namespace TechExpoWorld.Areas.Admin.Controllers
{
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using TechExpoWorld.Areas.Admin.Models.Events;
    using TechExpoWorld.Infrastructure.Extensions;
    using TechExpoWorld.Services.Events;

    using static GlobalConstants.TempData;

    public class EventsController : AdminController
    {
        private const string ControllerEvents = "Events";
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
        public async Task<IActionResult> Add(EventFormModel eventData)
        {
            if (!ModelState.IsValid)
            {
                return View(eventData);
            }

            var userId = this.User.Id();

            await this.events.CreateEventWithTickets(
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

            return RedirectToAction(nameof(TechExpoWorld.Controllers.EventsController.All),
                                    ControllerEvents,
                                    new { area = string.Empty });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var eventData = await this.events.Details(id);

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
            var eventExists = await this.events.EventExists(id);

            if (!eventExists)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(eventData);
            }

            await this.events.Edit(
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

            return RedirectToAction(nameof(TechExpoWorld.Controllers.EventsController.All),
                                    ControllerEvents,
                                    new { area = string.Empty });
        }

        public async Task<IActionResult> DeleteDetails(int id)
        {
            var eventData = await this.events.Details(id);

            if (eventData == null)
            {
                return NotFound();
            }

            return View(eventData);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var eventExists = await this.events.EventExists(id);

            if (!eventExists)
            {
                return NotFound();
            }

            await this.events.Delete(id);

            TempData[GlobalMessageKey] = DeletedEvent;

            return RedirectToAction(nameof(TechExpoWorld.Controllers.EventsController.All),
                                    ControllerEvents,
                                    new { area = string.Empty });
        }
    }
}
