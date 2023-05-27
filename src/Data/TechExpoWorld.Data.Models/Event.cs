namespace TechExpoWorld.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using TechExpoWorld.Data.Common.Models;

    using static TechExpoWorld.Common.GlobalConstants.Event;

    public class Event : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }

        [Required]
        [MaxLength(LocationMaxLength)]
        public string Location { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int TotalPhysicalTickets { get; set; }

        public int TotalVirtualTickets { get; set; }

        [Required]
        public string UserId { get; init; }

        public ApplicationUser User { get; init; }

        public IEnumerable<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
