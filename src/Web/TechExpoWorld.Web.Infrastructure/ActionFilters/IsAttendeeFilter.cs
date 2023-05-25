namespace TechExpoWorld.Web.Infrastructure.ActionFilters
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    using TechExpoWorld.Services.Data.Attendees;
    using TechExpoWorld.Web.Infrastructure.Extensions;

    public class IsAttendeeFilter : IAsyncActionFilter
    {
        private const string ActionBecomeAttendee = "BecomeAttendee";
        private const string ControllerAttendees = "Attendees";
        private readonly IAttendeesService attendeesService;

        public IsAttendeeFilter(IAttendeesService attendeesService)
            => this.attendeesService = attendeesService;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.User.IsAdmin())
            {
                context.Result = new BadRequestResult();
                return;
            }

            var attendeeId = await this.attendeesService.AttendeeIdAsync(context.HttpContext.User.Id());

            if (attendeeId == null)
            {
                context.Result = new RedirectToActionResult(
                    ActionBecomeAttendee,
                    ControllerAttendees,
                    null);
                return;
            }

            context.HttpContext.Items.Add(nameof(attendeeId), attendeeId);

            var resultContext = await next();
        }
    }
}
