﻿namespace TechExpoWorld.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Ticket;

    public class Ticket
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(TypeMaxLength)]
        public string Type { get; set; }

        [Precision(8, 2)]
        public decimal Price { get; set; }

        public bool IsSold { get; set; }

        public int EventId { get; set; }

        public Event Event { get; set; }

        public int? AttendeeId { get; set; }

        public Attendee Attendee { get; set; }
    }
}
