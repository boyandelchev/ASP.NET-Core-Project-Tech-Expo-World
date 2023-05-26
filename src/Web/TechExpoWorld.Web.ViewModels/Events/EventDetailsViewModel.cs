namespace TechExpoWorld.Web.ViewModels.Events
{
    using System.Globalization;

    using AutoMapper;

    using TechExpoWorld.Data.Models;
    using TechExpoWorld.Services.Mapping;

    using static TechExpoWorld.Common.GlobalConstants.System;

    public class EventDetailsViewModel : EventViewModel, IMapFrom<Event>, IHaveCustomMappings
    {
        public string Content { get; init; }

        public int TotalPhysicalTickets { get; init; }

        public int TotalVirtualTickets { get; init; }

        public int TotalAvailablePhysicalTickets { get; set; }

        public int TotalAvailableVirtualTickets { get; set; }

        public decimal PhysicalTicketPrice { get; set; }

        public decimal VirtualTicketPrice { get; set; }

        public new void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Event, EventDetailsViewModel>()
                .ForMember(
                    m => m.StartDate,
                    opt => opt.MapFrom(e => e.StartDate.ToString(DateTimeFormat, CultureInfo.InvariantCulture)))
                .ForMember(
                    m => m.EndDate,
                    opt => opt.MapFrom(e => e.EndDate.ToString(DateTimeFormat, CultureInfo.InvariantCulture)));
        }
    }
}
