namespace TechExpoWorld.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using TechExpoWorld.Data.Common.Models;

    using static TechExpoWorld.Common.GlobalConstants.Attendee;

    public class Attendee : BaseDeletableModel<string>
    {
        public Attendee()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; init; }

        [Required]
        [MaxLength(PhoneNumberMaxLength)]
        public string PhoneNumber { get; init; }

        [Required]
        [MaxLength(WorkEmailMaxLength)]
        public string WorkEmail { get; init; }

        [Required]
        [MaxLength(JobTitleMaxLength)]
        public string JobTitle { get; init; }

        [Required]
        [MaxLength(CompanyNameMaxLength)]
        public string CompanyName { get; init; }

        public int CountryId { get; init; }

        public Country Country { get; init; }

        public int JobTypeId { get; init; }

        public JobType JobType { get; init; }

        public int CompanyTypeId { get; init; }

        public CompanyType CompanyType { get; init; }

        public int CompanySectorId { get; init; }

        public CompanySector CompanySector { get; init; }

        public int CompanySizeId { get; init; }

        public CompanySize CompanySize { get; init; }

        [Required]
        public string UserId { get; init; }

        public IEnumerable<Ticket> Tickets { get; init; } = new List<Ticket>();
    }
}
