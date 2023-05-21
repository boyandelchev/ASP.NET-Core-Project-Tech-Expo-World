namespace TechExpoWorld.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static GlobalConstants.CompanyType;

    public class CompanyType
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        public IEnumerable<Attendee> Attendees { get; init; } = new List<Attendee>();
    }
}
