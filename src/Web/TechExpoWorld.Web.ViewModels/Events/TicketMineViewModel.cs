namespace TechExpoWorld.Web.ViewModels.Events
{
    using AutoMapper;

    using TechExpoWorld.Data.Models;
    using TechExpoWorld.Services.Mapping;

    public class TicketMineViewModel : IMapFrom<Ticket>, IHaveCustomMappings
    {
        public int EventId { get; init; }

        public string EventTitle { get; init; }

        public int TicketId { get; init; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Ticket, TicketMineViewModel>()
                .ForMember(
                    m => m.TicketId,
                    opt => opt.MapFrom(t => t.Id));
        }
    }
}
