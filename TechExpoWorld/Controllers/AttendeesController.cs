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
        private readonly IAttendeesService attendees;

        public AttendeesController(IAttendeesService attendees)
            => this.attendees = attendees;

        [Authorize]
        public async Task<IActionResult> BecomeAttendee()
        {
            if (await this.attendees.IsAttendeeAsync(this.User.Id()) || this.User.IsAdmin())
            {
                return BadRequest();
            }

            return View(new BecomeAttendeeFormModel
            {
                Countries = await this.attendees.CountriesAsync(),
                JobTypes = await this.attendees.JobTypesAsync(),
                CompanyTypes = await this.attendees.CompanyTypesAsync(),
                CompanySectors = await this.attendees.CompanySectorsAsync(),
                CompanySizes = await this.attendees.CompanySizesAsync()
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> BecomeAttendee(BecomeAttendeeFormModel attendee)
        {
            var userId = this.User.Id();

            if (await this.attendees.IsAttendeeAsync(userId) || this.User.IsAdmin())
            {
                return BadRequest();
            }

            if (!await this.attendees.CountryExistsAsync(attendee.CountryId))
            {
                this.ModelState.AddModelError(nameof(attendee.CountryId), ErrorCountry);
            }

            if (!await this.attendees.JobTypeExistsAsync(attendee.JobTypeId))
            {
                this.ModelState.AddModelError(nameof(attendee.JobTypeId), ErrorJobType);
            }

            if (!await this.attendees.CompanyTypeExistsAsync(attendee.CompanyTypeId))
            {
                this.ModelState.AddModelError(nameof(attendee.CompanyTypeId), ErrorCompanyType);
            }

            if (!await this.attendees.CompanySectorExistsAsync(attendee.CompanySectorId))
            {
                this.ModelState.AddModelError(nameof(attendee.CompanySectorId), ErrorCompanySector);
            }

            if (!await this.attendees.CompanySizeExistsAsync(attendee.CompanySizeId))
            {
                this.ModelState.AddModelError(nameof(attendee.CompanySizeId), ErrorCompanySize);
            }

            if (!ModelState.IsValid)
            {
                attendee.Countries = await this.attendees.CountriesAsync();
                attendee.JobTypes = await this.attendees.JobTypesAsync();
                attendee.CompanyTypes = await this.attendees.CompanyTypesAsync();
                attendee.CompanySectors = await this.attendees.CompanySectorsAsync();
                attendee.CompanySizes = await this.attendees.CompanySizesAsync();

                return View(attendee);
            }

            await this.attendees.CreateAsync(
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
