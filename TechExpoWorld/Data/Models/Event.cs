namespace TechExpoWorld.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Event;

    public class Event
    {
        public int Id { get; init; }

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

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }

        public IEnumerable<EventAttendee> EventAttendees { get; set; } = new List<EventAttendee>();

        public IEnumerable<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
