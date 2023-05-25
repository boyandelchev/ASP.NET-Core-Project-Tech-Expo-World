namespace TechExpoWorld.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using TechExpoWorld.Services.Data.Events;
    using TechExpoWorld.Web.Infrastructure.Extensions;
    using TechExpoWorld.Web.ViewModels.Events;

    using static TechExpoWorld.Common.GlobalConstants.TempData;

    public class EventsController : AdministrationController
    {
        private const string ControllerEvents = "Events";
        private readonly IEventsService eventsService;

        public EventsController(IEventsService eventsService)
            => this.eventsService = eventsService;

        public IActionResult Add() => this.View();

        [HttpPost]
        public async Task<IActionResult> Add(EventInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var userId = this.User.Id();

            await this.eventsService.CreateEventWithTicketsAsync(
                input.Title,
                input.Content,
                input.Location,
                input.StartDate,
                input.EndDate,
                input.TotalPhysicalTickets,
                input.PhysicalTicketPrice,
                input.TotalVirtualTickets,
                input.VirtualTicketPrice,
                userId);

            this.TempData[GlobalMessageKey] = CreatedEvent;

            return this.RedirectToAction(
                nameof(TechExpoWorld.Web.Controllers.EventsController.All),
                ControllerEvents,
                new { area = string.Empty });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var input = await this.eventsService.DetailsAsync<EventInputModel>(id);

            if (input == null)
            {
                return this.NotFound();
            }

            input.PhysicalTicketPrice = await this.eventsService.PhysicalTicketPriceAsync(id);
            input.VirtualTicketPrice = await this.eventsService.VirtualTicketPriceAsync(id);

            return this.View(input);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EventInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.eventsService.EditAsync(
                 id,
                 input.Title,
                 input.Content,
                 input.Location,
                 input.StartDate,
                 input.EndDate,
                 input.TotalPhysicalTickets,
                 input.PhysicalTicketPrice,
                 input.TotalVirtualTickets,
                 input.VirtualTicketPrice);

            this.TempData[GlobalMessageKey] = EditedEvent;

            return this.RedirectToAction(
                nameof(TechExpoWorld.Web.Controllers.EventsController.All),
                ControllerEvents,
                new { area = string.Empty });
        }

        public async Task<IActionResult> Delete(int id) => await this.Edit(id);

        [ActionName(nameof(Delete))]
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await this.eventsService.DeleteAsync(id);

            this.TempData[GlobalMessageKey] = DeletedEvent;

            return this.RedirectToAction(
                nameof(TechExpoWorld.Web.Controllers.EventsController.All),
                ControllerEvents,
                new { area = string.Empty });
        }
    }
}
