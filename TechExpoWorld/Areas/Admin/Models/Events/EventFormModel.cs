namespace TechExpoWorld.Areas.Admin.Models.Events
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Event;

    public class EventFormModel
    {
        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; init; }

        [Required]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength)]
        public string Content { get; init; }

        [Required]
        [StringLength(LocationMaxLength, MinimumLength = LocationMinLength)]
        public string Location { get; init; }

        [Required]
        [IsDateTimeBefore(nameof(EndDate))]
        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; init; }

        [Required]
        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; init; }

        [Range(1, 2000)]
        [Display(Name = "Total Physical Tickets")]
        public int TotalPhysicalTickets { get; init; }

        [Range(1, 2000)]
        [Display(Name = "Total Virtual Tickets")]
        public int TotalVirtualTickets { get; init; }

        [Range(0, 100000)]
        [Display(Name = "Price - Physical")]
        public decimal PhysicalTicketPrice { get; init; }

        [Range(0, 100000)]
        [Display(Name = "Price - Virtual")]
        public decimal VirtualTicketPrice { get; init; }
    }
}
