namespace TechExpoWorld.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TechExpoWorld.Infrastructure;
    using TechExpoWorld.Models.Attendees;
    using TechExpoWorld.Services.Attendees;

    public class AttendeesController : Controller
    {
        private readonly IAttendeeService attendees;

        public AttendeesController(IAttendeeService attendees)
            => this.attendees = attendees;

        [Authorize]
        public IActionResult BecomeAttendee()
        {
            var userId = this.User.Id();

            if (this.attendees.IsAttendee(userId) || this.User.IsAdmin())
            {
                return BadRequest();
            }

            return View(new BecomeAttendeeFormModel
            {
                JobTypes = this.attendees.JobTypes(),
                CompanyTypes = this.attendees.CompanyTypes(),
                CompanySectors = this.attendees.CompanySectors(),
                CompanySizes = this.attendees.CompanySizes()
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult BecomeAttendee(BecomeAttendeeFormModel attendee)
        {
            var userId = this.User.Id();

            if (this.attendees.IsAttendee(userId) || this.User.IsAdmin())
            {
                return BadRequest();
            }

            if (!this.attendees.JobTypeExists(attendee.JobTypeId))
            {
                this.ModelState.AddModelError(nameof(attendee.JobTypeId), "Job type does not exist.");
            }

            if (!this.attendees.CompanyTypeExists(attendee.CompanyTypeId))
            {
                this.ModelState.AddModelError(nameof(attendee.CompanyTypeId), "Company type does not exist.");
            }

            if (!this.attendees.CompanySectorExists(attendee.CompanySectorId))
            {
                this.ModelState.AddModelError(nameof(attendee.CompanySectorId), "Company sector does not exist.");
            }

            if (!this.attendees.CompanySizeExists(attendee.CompanySizeId))
            {
                this.ModelState.AddModelError(nameof(attendee.CompanySizeId), "Company size does not exist.");
            }

            if (!ModelState.IsValid)
            {
                return View(new BecomeAttendeeFormModel
                {
                    JobTypes = this.attendees.JobTypes(),
                    CompanyTypes = this.attendees.CompanyTypes(),
                    CompanySectors = this.attendees.CompanySectors(),
                    CompanySizes = this.attendees.CompanySizes()
                });
            }

            this.attendees.Create(
                attendee.Name,
                attendee.PhoneNumber,
                attendee.WorkEmail,
                attendee.JobTitle,
                attendee.CompanyName,
                attendee.Country,
                attendee.JobTypeId,
                attendee.CompanyTypeId,
                attendee.CompanySectorId,
                attendee.CompanySizeId,
                userId);

            return RedirectToAction(nameof(EventsController.All), "Events");
        }
    }
}
