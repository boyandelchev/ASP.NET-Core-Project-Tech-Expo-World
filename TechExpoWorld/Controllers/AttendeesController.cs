namespace TechExpoWorld.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TechExpoWorld.Infrastructure.Extensions;
    using TechExpoWorld.Models.Attendees;
    using TechExpoWorld.Services.Attendees;

    using static GlobalConstants.Attendee;
    using static GlobalConstants.TempData;

    public class AttendeesController : Controller
    {
        private const string ControllerEvents = "Events";
        private readonly IAttendeeService attendees;

        public AttendeesController(IAttendeeService attendees)
            => this.attendees = attendees;

        [Authorize]
        public async Task<IActionResult> BecomeAttendee()
        {
            if (await this.attendees.IsAttendee(this.User.Id()) || this.User.IsAdmin())
            {
                return BadRequest();
            }

            return View(new BecomeAttendeeFormModel
            {
                Countries = await this.attendees.Countries(),
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

            if (!await this.attendees.CountryExists(attendee.CountryId))
            {
                this.ModelState.AddModelError(nameof(attendee.CountryId), ErrorCountry);
            }

            if (!await this.attendees.JobTypeExists(attendee.JobTypeId))
            {
                this.ModelState.AddModelError(nameof(attendee.JobTypeId), ErrorJobType);
            }

            if (!await this.attendees.CompanyTypeExists(attendee.CompanyTypeId))
            {
                this.ModelState.AddModelError(nameof(attendee.CompanyTypeId), ErrorCompanyType);
            }

            if (!await this.attendees.CompanySectorExists(attendee.CompanySectorId))
            {
                this.ModelState.AddModelError(nameof(attendee.CompanySectorId), ErrorCompanySector);
            }

            if (!await this.attendees.CompanySizeExists(attendee.CompanySizeId))
            {
                this.ModelState.AddModelError(nameof(attendee.CompanySizeId), ErrorCompanySize);
            }

            if (!ModelState.IsValid)
            {
                attendee.Countries = await this.attendees.Countries();
                attendee.JobTypes = await this.attendees.JobTypes();
                attendee.CompanyTypes = await this.attendees.CompanyTypes();
                attendee.CompanySectors = await this.attendees.CompanySectors();
                attendee.CompanySizes = await this.attendees.CompanySizes();

                return View(attendee);
            }

            await this.attendees.Create(
                attendee.Name,
                attendee.PhoneNumber,
                attendee.WorkEmail,
                attendee.JobTitle,
                attendee.CompanyName,
                attendee.CountryId,
                attendee.JobTypeId,
                attendee.CompanyTypeId,
                attendee.CompanySectorId,
                attendee.CompanySizeId,
                userId);

            TempData[GlobalMessageKey] = CreatedAttendee;

            return RedirectToAction(nameof(EventsController.All), ControllerEvents);
        }
    }
}
