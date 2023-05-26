namespace TechExpoWorld.Web.ViewModels.News
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using AutoMapper;

    using TechExpoWorld.Data.Models;
    using TechExpoWorld.Services.Mapping;

    using static TechExpoWorld.Common.GlobalConstants.NewsArticle;

    public class NewsArticleInputModel : INewsArticleModel, IMapFrom<NewsArticle>, IHaveCustomMappings
    {
        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; init; }

        [Required]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength)]
        public string Content { get; init; }

        [Required]
        [Url]
        [Display(Name = DisplayImageUrl)]
        public string ImageUrl { get; init; }

        [Display(Name = DisplaySelectCategory)]
        public int CategoryId { get; init; }

        [Display(Name = DisplaySelectTags)]
        public IEnumerable<int> TagIds { get; init; } = new List<int>();

        public IEnumerable<CategoryViewModel> Categories { get; set; }

        public IEnumerable<TagViewModel> Tags { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<NewsArticle, NewsArticleInputModel>()
                .ForMember(
                    m => m.TagIds,
                    opt => opt.MapFrom(na => na.NewsArticleTags.Select(nat => nat.TagId)));
        }
    }
}
