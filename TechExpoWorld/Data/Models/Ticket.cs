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
        public string Type { get; set; }

        [Precision(PricePrecision, PriceScale)]
        public decimal Price { get; set; }

        public bool IsSold { get; set; }

        public int EventId { get; set; }

        public Event Event { get; set; }

        public int? AttendeeId { get; set; }

        public Attendee Attendee { get; set; }
    }
}
