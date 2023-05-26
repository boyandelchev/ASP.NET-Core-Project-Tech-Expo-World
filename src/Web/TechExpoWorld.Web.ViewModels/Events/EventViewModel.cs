namespace TechExpoWorld.Web.ViewModels.Events
{
    using System.Globalization;

    using AutoMapper;

    using TechExpoWorld.Data.Models;
    using TechExpoWorld.Services.Mapping;

    using static TechExpoWorld.Common.GlobalConstants.System;

    public class EventViewModel : IEventModel, IMapFrom<Event>, IHaveCustomMappings
    {
        public int Id { get; init; }

        public string Title { get; init; }

        public string Location { get; init; }

        public string StartDate { get; init; }

        public string EndDate { get; init; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Event, EventViewModel>()
                .ForMember(
                    m => m.StartDate,
                    opt => opt.MapFrom(e => e.StartDate.ToString(DateTimeFormat, CultureInfo.InvariantCulture)))
                .ForMember(
                    m => m.EndDate,
                    opt => opt.MapFrom(e => e.EndDate.ToString(DateTimeFormat, CultureInfo.InvariantCulture)));
        }
    }
}
