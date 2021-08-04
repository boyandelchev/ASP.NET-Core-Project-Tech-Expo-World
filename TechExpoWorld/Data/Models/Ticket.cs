namespace TechExpoWorld.Data.Models
{
    public class Ticket
    {
        public int Id { get; init; }

        public decimal Price { get; set; }

        public int EventId { get; set; }

        public Event Event { get; set; }

        public int? AttendeeId { get; set; }

        public Attendee Attendee { get; set; }

        public int TicketTypeId { get; set; }

        public TicketType TicketType { get; set; }
    }
}
