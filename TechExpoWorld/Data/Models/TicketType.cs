namespace TechExpoWorld.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.TicketType;

    public class TicketType
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        public IEnumerable<Ticket> Tickets { get; init; } = new List<Ticket>();
    }
}
