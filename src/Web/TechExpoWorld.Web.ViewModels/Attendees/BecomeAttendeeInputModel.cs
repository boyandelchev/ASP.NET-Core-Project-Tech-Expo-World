namespace TechExpoWorld.Web.ViewModels.Attendees
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static TechExpoWorld.Common.GlobalConstants.Attendee;

    public class BecomeAttendeeInputModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; init; }

        [Required]
        [StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMinLength)]
        [Display(Name = DisplayPhoneNumber)]
        public string PhoneNumber { get; init; }

        [Required]
        [StringLength(WorkEmailMaxLength, MinimumLength = WorkEmailMinLength)]
        [EmailAddress]
        [Display(Name = DisplayWorkEmail)]
        public string WorkEmail { get; init; }

        [Required]
        [StringLength(JobTitleMaxLength, MinimumLength = JobTitleMinLength)]
        [Display(Name = DisplayJobTitle)]
        public string JobTitle { get; init; }

        [Required]
        [StringLength(CompanyNameMaxLength, MinimumLength = CompanyNameMinLength)]
        [Display(Name = DisplayCompanyName)]
        public string CompanyName { get; init; }

        [Display(Name = DisplayCountry)]
        public int CountryId { get; init; }

        [Display(Name = DisplayJobType)]
        public int JobTypeId { get; init; }

        [Display(Name = DisplayCompanyType)]
        public int CompanyTypeId { get; init; }

        [Display(Name = DisplayCompanySector)]
        public int CompanySectorId { get; init; }

        [Display(Name = DisplayCompanySize)]
        public int CompanySizeId { get; init; }

        public IEnumerable<CountryViewModel> Countries { get; set; }

        public IEnumerable<JobTypeViewModel> JobTypes { get; set; }

        public IEnumerable<CompanyTypeViewModel> CompanyTypes { get; set; }

        public IEnumerable<CompanySectorViewModel> CompanySectors { get; set; }

        public IEnumerable<CompanySizeViewModel> CompanySizes { get; set; }
    }
}
