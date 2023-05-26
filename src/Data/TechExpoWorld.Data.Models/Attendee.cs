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
        public string Name { get; set; }

        [Required]
        [MaxLength(PhoneNumberMaxLength)]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(WorkEmailMaxLength)]
        public string WorkEmail { get; set; }

        [Required]
        [MaxLength(JobTitleMaxLength)]
        public string JobTitle { get; set; }

        [Required]
        [MaxLength(CompanyNameMaxLength)]
        public string CompanyName { get; set; }

        public int CountryId { get; set; }

        public Country Country { get; set; }

        public int JobTypeId { get; set; }

        public JobType JobType { get; set; }

        public int CompanyTypeId { get; set; }

        public CompanyType CompanyType { get; set; }

        public int CompanySectorId { get; set; }

        public CompanySector CompanySector { get; set; }

        public int CompanySizeId { get; set; }

        public CompanySize CompanySize { get; set; }

        [Required]
        public string UserId { get; set; }

        public IEnumerable<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
