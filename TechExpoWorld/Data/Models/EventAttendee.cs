namespace TechExpoWorld.Data.Models
{
    public class EventAttendee
    {
        public int EventId { get; set; }

        public Event Event { get; set; }

        public int AttendeeId { get; set; }

        public Attendee Attendee { get; set; }
    }
}
