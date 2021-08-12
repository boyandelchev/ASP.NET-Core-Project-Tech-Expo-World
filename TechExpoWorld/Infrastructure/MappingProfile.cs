namespace TechExpoWorld.Infrastructure
{
    using System.Globalization;
    using System.Linq;
    using AutoMapper;
    using TechExpoWorld.Areas.Admin.Models.Events;
    using TechExpoWorld.Data.Models;
    using TechExpoWorld.Models.Home;
    using TechExpoWorld.Models.News;
    using TechExpoWorld.Services.Attendees.Models;
    using TechExpoWorld.Services.Comments.Models;
    using TechExpoWorld.Services.Events.Models;
    using TechExpoWorld.Services.News.Models;
    using TechExpoWorld.Services.Statistics.Models;

    public class MappingProfile : Profile
    {
        private const string Ellipsis = "...";
        private const string DateFormatWithTime = "dd.MM.yyyy HH:mm";
        private const string DateFormatNoTime = "dd.MM.yyyy";

        public MappingProfile()
        {
            this.CreateMap<NewsArticle, NewsArticleIndexServiceModel>();

            this.CreateMap<StatisticsServiceModel, IndexViewModel>();

            this.CreateMap<NewsArticle, NewsArticleServiceModel>()
                .ForMember(na => na.Content, cfg => cfg.MapFrom(na => na.Content.Substring(0, 200) + Ellipsis))
                .ForMember(na => na.CreatedOn, cfg => cfg.MapFrom(na => na.CreatedOn.ToString(DateFormatWithTime, CultureInfo.InvariantCulture)))
                .ForMember(na => na.CategoryName, cfg => cfg.MapFrom(na => na.NewsCategory.Name));

            this.CreateMap<NewsArticle, NewsArticleDetailsServiceModel>()
                .ForMember(na => na.CreatedOn, cfg => cfg.MapFrom(na => na.CreatedOn.ToString(DateFormatWithTime, CultureInfo.InvariantCulture)))
                .ForMember(na => na.LastModifiedOn, cfg => cfg.MapFrom(na => na.LastModifiedOn.Value.ToString(DateFormatWithTime, CultureInfo.InvariantCulture)))
                .ForMember(na => na.CategoryId, cfg => cfg.MapFrom(na => na.NewsCategoryId))
                .ForMember(na => na.CategoryName, cfg => cfg.MapFrom(na => na.NewsCategory.Name))
                .ForMember(na => na.TagIds, cfg => cfg.MapFrom(na => na.NewsArticleTags.Select(nat => nat.TagId)))
                .ForMember(na => na.TagNames, cfg => cfg.MapFrom(na => na.NewsArticleTags.Select(nat => nat.Tag.Name)))
                .ForMember(na => na.UserId, cfg => cfg.MapFrom(na => na.Author.UserId));

            this.CreateMap<NewsCategory, CategoryServiceModel>();

            this.CreateMap<Tag, TagServiceModel>();

            this.CreateMap<NewsArticlesQueryServiceModel, AllNewsQueryModel>();

            this.CreateMap<NewsArticleDetailsServiceModel, NewsArticleDetailsViewModel>();

            this.CreateMap<NewsArticleDetailsServiceModel, NewsArticleFormModel>();

            this.CreateMap<NewsArticleDetailsServiceModel, NewsArticleDeleteDetailsViewModel>();

            this.CreateMap<Comment, CommentServiceModel>()
                .ForMember(c => c.CreatedOn, cfg => cfg.MapFrom(c => c.CreatedOn.ToString(DateFormatWithTime, CultureInfo.InvariantCulture)))
                .ForMember(c => c.UserName, cfg => cfg.MapFrom(c => c.User.UserName));

            this.CreateMap<JobType, JobTypeServiceModel>();

            this.CreateMap<CompanyType, CompanyTypeServiceModel>();

            this.CreateMap<CompanySector, CompanySectorServiceModel>();

            this.CreateMap<CompanySize, CompanySizeServiceModel>();

            this.CreateMap<Event, EventServiceModel>()
                .ForMember(e => e.StartDate, cfg => cfg.MapFrom(e => e.StartDate.ToString(DateFormatNoTime, CultureInfo.InvariantCulture)))
                .ForMember(e => e.EndDate, cfg => cfg.MapFrom(e => e.EndDate.ToString(DateFormatNoTime, CultureInfo.InvariantCulture)));

            this.CreateMap<Event, EventDetailsServiceModel>()
                .ForMember(e => e.StartDate, cfg => cfg.MapFrom(e => e.StartDate.ToString(DateFormatNoTime, CultureInfo.InvariantCulture)))
                .ForMember(e => e.EndDate, cfg => cfg.MapFrom(e => e.EndDate.ToString(DateFormatNoTime, CultureInfo.InvariantCulture)));

            this.CreateMap<EventDetailsServiceModel, EventFormModel>();

            this.CreateMap<Ticket, MyTicketServiceModel>()
                .ForMember(t => t.TicketId, cfg => cfg.MapFrom(t => t.Id));
        }
    }
}
