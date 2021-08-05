namespace TechExpoWorld.Models.Events
{
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
        [RegularExpression(DateRegularExpression, ErrorMessage = "The field Start Date must be in format '15.08.2021'!")]
        [Display(Name = "Start Date")]
        public string StartDate { get; init; }

        [Required]
        [RegularExpression(DateRegularExpression, ErrorMessage = "The field End Date must be in format '15.08.2021'!")]
        [Display(Name = "End Date")]
        public string EndDate { get; init; }

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
