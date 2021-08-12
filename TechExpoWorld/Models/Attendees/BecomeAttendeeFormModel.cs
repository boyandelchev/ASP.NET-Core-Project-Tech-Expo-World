namespace TechExpoWorld.Models.Attendees
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using TechExpoWorld.Services.Attendees.Models;

    using static Data.DataConstants.Attendee;

    public class BecomeAttendeeFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; init; }

        [Required]
        [StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMinLength)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; init; }

        [Required]
        [StringLength(WorkEmailMaxLength, MinimumLength = WorkEmailMinLength)]
        [EmailAddress]
        [Display(Name = "Work Email")]
        public string WorkEmail { get; init; }

        [Required]
        [StringLength(JobTitleMaxLength, MinimumLength = JobTitleMinLength)]
        [Display(Name = "Job Title")]
        public string JobTitle { get; init; }

        [Required]
        [StringLength(CompanyNameMaxLength, MinimumLength = CompanyNameMinLength)]
        [Display(Name = "Company Name")]
        public string CompanyName { get; init; }

        [Required]
        [StringLength(CountryMaxLength, MinimumLength = CountryMinLength)]
        public string Country { get; init; }

        [Display(Name = "Job Type")]
        public int JobTypeId { get; init; }

        [Display(Name = "Company Type")]
        public int CompanyTypeId { get; init; }

        [Display(Name = "Company Sector")]
        public int CompanySectorId { get; init; }

        [Display(Name = "Company Size")]
        public int CompanySizeId { get; init; }

        public IEnumerable<JobTypeServiceModel> JobTypes { get; init; }

        public IEnumerable<CompanyTypeServiceModel> CompanyTypes { get; init; }

        public IEnumerable<CompanySectorServiceModel> CompanySectors { get; init; }

        public IEnumerable<CompanySizeServiceModel> CompanySizes { get; init; }
    }
}
