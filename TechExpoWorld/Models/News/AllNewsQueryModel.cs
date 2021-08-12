namespace TechExpoWorld.Models.News
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using TechExpoWorld.Services.News.Models;

    public class AllNewsQueryModel
    {
        public const int NewsArticlesPerPage = 3;

        public int CurrentPage { get; init; } = 1;

        public int TotalNewsArticles { get; set; }

        public string Category { get; init; }

        public string Tag { get; init; }

        [Display(Name = "Search by text")]
        public string SearchTerm { get; init; }

        [Display(Name = "Sort by date")]
        public NewsSorting Sorting { get; init; }

        public IEnumerable<string> Categories { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public IEnumerable<NewsArticleServiceModel> News { get; set; }
    }
}
