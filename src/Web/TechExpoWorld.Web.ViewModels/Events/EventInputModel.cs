namespace TechExpoWorld.Web.ViewModels.Events
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static TechExpoWorld.Common.GlobalConstants.Event;

    public class EventInputModel
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
        [Display(Name = DisplayStartDate)]
        [DisplayFormat(DataFormatString = DisplayFormatDateTime, ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; init; }

        [Required]
        [Display(Name = DisplayEndDate)]
        [DisplayFormat(DataFormatString = DisplayFormatDateTime, ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; init; }

        [Range(TicketCountMinRange, TicketCountMaxRange)]
        [Display(Name = DisplayTotalPhysicalTickets)]
        public int TotalPhysicalTickets { get; init; }

        [Range(TicketCountMinRange, TicketCountMaxRange)]
        [Display(Name = DisplayTotalVirtualTickets)]
        public int TotalVirtualTickets { get; init; }

        [Range(TicketPriceMinRange, TicketPriceMaxRange)]
        [Display(Name = DisplayPricePhysical)]
        public decimal PhysicalTicketPrice { get; set; }

        [Range(TicketPriceMinRange, TicketPriceMaxRange)]
        [Display(Name = DisplayPriceVirtual)]
        public decimal VirtualTicketPrice { get; set; }
    }
}
