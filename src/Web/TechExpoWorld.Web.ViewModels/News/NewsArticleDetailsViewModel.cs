namespace TechExpoWorld.Web.ViewModels.News
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using AutoMapper;

    using TechExpoWorld.Data.Models;
    using TechExpoWorld.Services.Mapping;

    using static TechExpoWorld.Common.GlobalConstants.System;

    public class NewsArticleDetailsViewModel : NewsArticleViewModel, IMapFrom<NewsArticle>, IHaveCustomMappings
    {
        public string ModifiedOn { get; init; }

        public int ViewCount { get; init; }

        public IEnumerable<string> TagNames { get; init; }

        public new void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<NewsArticle, NewsArticleDetailsViewModel>()
                .ForMember(
                    m => m.CreatedOn,
                    opt => opt.MapFrom(na => na.CreatedOn.ToString(DateTimeFormat, CultureInfo.InvariantCulture)))
                .ForMember(
                    m => m.ModifiedOn,
                    opt => opt.MapFrom(na => na.ModifiedOn.Value.ToString(DateTimeFormat, CultureInfo.InvariantCulture)))
                .ForMember(
                    m => m.TagNames,
                    opt => opt.MapFrom(na => na.NewsArticleTags.Select(nat => nat.Tag.Name)));
        }
    }
}
