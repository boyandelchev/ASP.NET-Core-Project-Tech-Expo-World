namespace TechExpoWorld.Infrastructure.MappingProfiles
{
    using System.Globalization;
    using System.Linq;

    using AutoMapper;

    using TechExpoWorld.Data.Models;
    using TechExpoWorld.Models.News;
    using TechExpoWorld.Services.Comments.Models;
    using TechExpoWorld.Services.News.Models;

    public class NewsProfile : Profile
    {
        private const string Ellipsis = "...";
        private const string DateTimeFormat = "dd.MM.yyyy HH:mm";

        public NewsProfile()
        {
            this.CreateMap<NewsArticle, LatestNewsArticleServiceModel>();

            this.CreateMap<NewsArticle, NewsArticleServiceModel>()
                .ForMember(na => na.Content, cfg => cfg.MapFrom(na => na.Content.Substring(0, 200) + Ellipsis))
                .ForMember(na => na.CreatedOn, cfg => cfg.MapFrom(na => na.CreatedOn.ToString(DateTimeFormat, CultureInfo.InvariantCulture)));

            this.CreateMap<NewsArticle, NewsArticleDetailsServiceModel>()
                .ForMember(na => na.CreatedOn, cfg => cfg.MapFrom(na => na.CreatedOn.ToString(DateTimeFormat, CultureInfo.InvariantCulture)))
                .ForMember(na => na.LastModifiedOn, cfg => cfg.MapFrom(na => na.LastModifiedOn.Value.ToString(DateTimeFormat, CultureInfo.InvariantCulture)))
                .ForMember(na => na.TagNames, cfg => cfg.MapFrom(na => na.NewsArticleTags.Select(nat => nat.Tag.Name)));

            this.CreateMap<NewsArticle, NewsArticleFormServiceModel>()
                .ForMember(na => na.TagIds, cfg => cfg.MapFrom(na => na.NewsArticleTags.Select(nat => nat.TagId)));

            this.CreateMap<NewsArticleFormServiceModel, NewsArticleFormModel>();

            this.CreateMap<Category, CategoryServiceModel>();

            this.CreateMap<Tag, TagServiceModel>();

            this.CreateMap<Comment, CommentServiceModel>()
                .ForMember(c => c.CreatedOn, cfg => cfg.MapFrom(c => c.CreatedOn.ToString(DateTimeFormat, CultureInfo.InvariantCulture)))
                .ForMember(c => c.UserName, cfg => cfg.MapFrom(c => c.User.UserName));
        }
    }
}
