namespace TechExpoWorld.Infrastructure.MappingProfiles
{
    using System;
    using System.Globalization;
    using AutoMapper;
    using TechExpoWorld.Data.Models;
    using TechExpoWorld.Models.Events;
    using TechExpoWorld.Services.Attendees.Models;
    using TechExpoWorld.Services.Events.Models;

    public class EventsProfile : Profile
    {
        private const string DateTimeFormat = "dd.MM.yyyy HH:mm";

        public EventsProfile()
        {
            this.CreateMap<Country, CountryServiceModel>();

            this.CreateMap<JobType, JobTypeServiceModel>();

            this.CreateMap<CompanyType, CompanyTypeServiceModel>();

            this.CreateMap<CompanySector, CompanySectorServiceModel>();

            this.CreateMap<CompanySize, CompanySizeServiceModel>();

            this.CreateMap<Event, EventServiceModel>()
                .ForMember(e => e.StartDate, cfg => cfg.MapFrom(e => e.StartDate.ToString(DateTimeFormat, CultureInfo.InvariantCulture)))
                .ForMember(e => e.EndDate, cfg => cfg.MapFrom(e => e.EndDate.ToString(DateTimeFormat, CultureInfo.InvariantCulture)));

            this.CreateMap<Event, EventDetailsServiceModel>()
                .ForMember(e => e.StartDate, cfg => cfg.MapFrom(e => e.StartDate.ToString(DateTimeFormat, CultureInfo.InvariantCulture)))
                .ForMember(e => e.EndDate, cfg => cfg.MapFrom(e => e.EndDate.ToString(DateTimeFormat, CultureInfo.InvariantCulture)));

            this.CreateMap<EventDetailsServiceModel, EventFormModel>()
                .ForMember(e => e.StartDate, cfg => cfg.MapFrom(e => DateTime.ParseExact(e.StartDate, DateTimeFormat, CultureInfo.InvariantCulture)))
                .ForMember(e => e.EndDate, cfg => cfg.MapFrom(e => DateTime.ParseExact(e.EndDate, DateTimeFormat, CultureInfo.InvariantCulture)));

            this.CreateMap<Ticket, MyTicketServiceModel>()
                .ForMember(t => t.TicketId, cfg => cfg.MapFrom(t => t.Id));
        }
    }
}
