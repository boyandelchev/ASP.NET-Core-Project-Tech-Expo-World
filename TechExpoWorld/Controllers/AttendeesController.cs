namespace TechExpoWorld.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TechExpoWorld.Infrastructure.Extensions;
    using TechExpoWorld.Models.Attendees;
    using TechExpoWorld.Services.Attendees;

    using static WebConstants;

    public class AttendeesController : Controller
    {
        private readonly IAttendeeService attendees;

        public AttendeesController(IAttendeeService attendees)
            => this.attendees = attendees;

        [Authorize]
        public async Task<IActionResult> BecomeAttendee()
        {
            var userId = this.User.Id();

            if (await this.attendees.IsAttendee(userId) || this.User.IsAdmin())
            {
                return BadRequest();
            }

            return View(new BecomeAttendeeFormModel
            {
                JobTypes = await this.attendees.JobTypes(),
                CompanyTypes = await this.attendees.CompanyTypes(),
                CompanySectors = await this.attendees.CompanySectors(),
                CompanySizes = await this.attendees.CompanySizes()
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> BecomeAttendee(BecomeAttendeeFormModel attendee)
        {
            var userId = this.User.Id();

            if (await this.attendees.IsAttendee(userId) || this.User.IsAdmin())
            {
                return BadRequest();
            }

            if (!await this.attendees.JobTypeExists(attendee.JobTypeId))
            {
                this.ModelState.AddModelError(nameof(attendee.JobTypeId), "Job type does not exist.");
            }

            if (!await this.attendees.CompanyTypeExists(attendee.CompanyTypeId))
            {
                this.ModelState.AddModelError(nameof(attendee.CompanyTypeId), "Company type does not exist.");
            }

            if (!await this.attendees.CompanySectorExists(attendee.CompanySectorId))
            {
                this.ModelState.AddModelError(nameof(attendee.CompanySectorId), "Company sector does not exist.");
            }

            if (!await this.attendees.CompanySizeExists(attendee.CompanySizeId))
            {
                this.ModelState.AddModelError(nameof(attendee.CompanySizeId), "Company size does not exist.");
            }

            if (!ModelState.IsValid)
            {
                return View(new BecomeAttendeeFormModel
                {
                    JobTypes = await this.attendees.JobTypes(),
                    CompanyTypes = await this.attendees.CompanyTypes(),
                    CompanySectors = await this.attendees.CompanySectors(),
                    CompanySizes = await this.attendees.CompanySizes()
                });
            }

            await this.attendees.Create(
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

            TempData[GlobalMessageKey] = "Thank you for becomming an attendee!";

            return RedirectToAction(nameof(EventsController.All), "Events");
        }
    }
}
