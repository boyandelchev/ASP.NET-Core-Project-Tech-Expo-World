namespace TechExpoWorld.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using TechExpoWorld.Services.Data.Attendees;
    using TechExpoWorld.Web.Infrastructure.Extensions;
    using TechExpoWorld.Web.ViewModels.Attendees;

    using static TechExpoWorld.Common.GlobalConstants.Attendee;
    using static TechExpoWorld.Common.GlobalConstants.TempData;

    public class AttendeesController : BaseController
    {
        private const string ControllerEvents = "Events";
        private readonly IAttendeesService attendeesService;

        public AttendeesController(IAttendeesService attendeesService)
            => this.attendeesService = attendeesService;

        [Authorize]
        public async Task<IActionResult> BecomeAttendee()
        {
            if (await this.attendeesService.IsAttendeeAsync(this.User.Id()) || this.User.IsAdmin())
            {
                return this.BadRequest();
            }

            return this.View(new BecomeAttendeeInputModel
            {
                Countries = await this.attendeesService.CountriesAsync<CountryViewModel>(),
                JobTypes = await this.attendeesService.JobTypesAsync<JobTypeViewModel>(),
                CompanyTypes = await this.attendeesService.CompanyTypesAsync<CompanyTypeViewModel>(),
                CompanySectors = await this.attendeesService.CompanySectorsAsync<CompanySectorViewModel>(),
                CompanySizes = await this.attendeesService.CompanySizesAsync<CompanySizeViewModel>(),
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> BecomeAttendee(BecomeAttendeeInputModel input)
        {
            var userId = this.User.Id();

            if (await this.attendeesService.IsAttendeeAsync(userId) || this.User.IsAdmin())
            {
                return this.BadRequest();
            }

            if (!await this.attendeesService.CountryExistsAsync(input.CountryId))
            {
                this.ModelState.AddModelError(nameof(input.CountryId), ErrorCountry);
            }

            if (!await this.attendeesService.JobTypeExistsAsync(input.JobTypeId))
            {
                this.ModelState.AddModelError(nameof(input.JobTypeId), ErrorJobType);
            }

            if (!await this.attendeesService.CompanyTypeExistsAsync(input.CompanyTypeId))
            {
                this.ModelState.AddModelError(nameof(input.CompanyTypeId), ErrorCompanyType);
            }

            if (!await this.attendeesService.CompanySectorExistsAsync(input.CompanySectorId))
            {
                this.ModelState.AddModelError(nameof(input.CompanySectorId), ErrorCompanySector);
            }

            if (!await this.attendeesService.CompanySizeExistsAsync(input.CompanySizeId))
            {
                this.ModelState.AddModelError(nameof(input.CompanySizeId), ErrorCompanySize);
            }

            if (!this.ModelState.IsValid)
            {
                input.Countries = await this.attendeesService.CountriesAsync<CountryViewModel>();
                input.JobTypes = await this.attendeesService.JobTypesAsync<JobTypeViewModel>();
                input.CompanyTypes = await this.attendeesService.CompanyTypesAsync<CompanyTypeViewModel>();
                input.CompanySectors = await this.attendeesService.CompanySectorsAsync<CompanySectorViewModel>();
                input.CompanySizes = await this.attendeesService.CompanySizesAsync<CompanySizeViewModel>();

                return this.View(input);
            }

            await this.attendeesService.CreateAsync(
                input.Name,
                input.PhoneNumber,
                input.WorkEmail,
                input.JobTitle,
                input.CompanyName,
                input.CountryId,
                input.JobTypeId,
                input.CompanyTypeId,
                input.CompanySectorId,
                input.CompanySizeId,
                userId);

            this.TempData[GlobalMessageKey] = CreatedAttendee;

            return this.RedirectToAction(nameof(EventsController.All), ControllerEvents);
        }
    }
}
