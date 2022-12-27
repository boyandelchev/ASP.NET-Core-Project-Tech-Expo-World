namespace TechExpoWorld.Models.News
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using TechExpoWorld.Services.News.Models;

    using static GlobalConstants.NewsArticle;

    public class NewsArticleDeleteDetailsViewModel
    {
        public int Id { get; init; }

        public string Title { get; init; }

        public string Content { get; init; }

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
