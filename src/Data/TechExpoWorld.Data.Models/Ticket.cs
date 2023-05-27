namespace TechExpoWorld.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.EntityFrameworkCore;

    using TechExpoWorld.Data.Common.Models;

    using static TechExpoWorld.Common.GlobalConstants.Ticket;

    public class Ticket : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(TypeMaxLength)]
        public string Type { get; init; }

        [Precision(PricePrecision, PriceScale)]
        public decimal Price { get; init; }

        public bool IsBooked { get; set; }

        public int? EventId { get; set; }

        public Event Event { get; init; }

        public string AttendeeId { get; set; }

        public Attendee Attendee { get; init; }
    }
}
