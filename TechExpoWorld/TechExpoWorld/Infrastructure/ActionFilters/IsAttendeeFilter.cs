namespace TechExpoWorld.Infrastructure.ActionFilters
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    using TechExpoWorld.Controllers;
    using TechExpoWorld.Infrastructure.Extensions;
    using TechExpoWorld.Services.Attendees;

    public class IsAttendeeFilter : IAsyncActionFilter
    {
        private const string ControllerAttendees = "Attendees";
        private readonly IAttendeesService attendees;

        public IsAttendeeFilter(IAttendeesService attendees)
            => this.attendees = attendees;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.User.IsAdmin())
            {
                context.Result = new BadRequestResult();
                return;
            }

            var attendeeId = await this.attendees.AttendeeIdAsync(context.HttpContext.User.Id());

            if (attendeeId == null)
            {
                context.Result = new RedirectToActionResult(
                    nameof(AttendeesController.BecomeAttendee),
                    ControllerAttendees,
                    null);
                return;
            }

            context.HttpContext.Items.Add(nameof(attendeeId), attendeeId);

            var resultContext = await next();
        }
    }
}
