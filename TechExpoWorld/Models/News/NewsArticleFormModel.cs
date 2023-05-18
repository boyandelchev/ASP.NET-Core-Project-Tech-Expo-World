namespace TechExpoWorld.Models.News
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using TechExpoWorld.Services.News.Models;

    using static GlobalConstants.NewsArticle;

    public class NewsArticleFormModel : INewsArticleModel
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

        public IEnumerable<CategoryServiceModel> Categories { get; set; }

        public IEnumerable<TagServiceModel> Tags { get; set; }
    }
}
