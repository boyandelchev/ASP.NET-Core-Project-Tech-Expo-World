namespace TechExpoWorld.Web.ViewModels.News
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static TechExpoWorld.Common.GlobalConstants.NewsArticle;

    public class NewsArticleInputModel : INewsArticleModel
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
    }
}
