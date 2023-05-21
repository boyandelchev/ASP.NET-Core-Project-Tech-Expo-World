namespace TechExpoWorld.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.EntityFrameworkCore;

    using static GlobalConstants.Ticket;

    public class Ticket
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(TypeMaxLength)]
        public string Type { get; init; }

        [Precision(PricePrecision, PriceScale)]
        public decimal Price { get; init; }

        public bool IsBooked { get; set; }

        public int? EventId { get; set; }

        public Event Event { get; set; }

        public string AttendeeId { get; set; }

        public Attendee Attendee { get; set; }
    }
}
