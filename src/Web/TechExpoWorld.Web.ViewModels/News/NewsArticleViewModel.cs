namespace TechExpoWorld.Web.ViewModels.News
{
    using System.Globalization;

    using AutoMapper;

    using TechExpoWorld.Data.Models;
    using TechExpoWorld.Services.Mapping;

    using static TechExpoWorld.Common.GlobalConstants.System;

    public class NewsArticleViewModel : INewsArticleModel, IMapFrom<NewsArticle>, IHaveCustomMappings
    {
        public int Id { get; init; }

        public string Title { get; init; }

        public string Content { get; init; }

        public string ImageUrl { get; init; }

        public string CreatedOn { get; init; }

        public string AuthorName { get; init; }

        public string CategoryName { get; init; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<NewsArticle, NewsArticleViewModel>()
                .ForMember(
                    m => m.Content,
                    opt => opt.MapFrom(na => na.Content.Substring(0, 200) + Ellipsis))
                .ForMember(
                    m => m.CreatedOn,
                    opt => opt.MapFrom(na => na.CreatedOn.ToString(DateTimeFormat, CultureInfo.InvariantCulture)));
        }
    }
}
