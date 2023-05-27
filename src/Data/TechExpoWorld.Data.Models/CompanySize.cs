namespace TechExpoWorld.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using TechExpoWorld.Data.Common.Models;

    using static TechExpoWorld.Common.GlobalConstants.CompanySize;

    public class CompanySize : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; init; }

        public IEnumerable<Attendee> Attendees { get; init; } = new List<Attendee>();
    }
}
