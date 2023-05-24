namespace TechExpoWorld.Web.ViewModels.News
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static TechExpoWorld.Common.GlobalConstants.NewsArticle;

    public class AllNewsQueryViewModel
    {
        public const int NewsArticlesPerPage = 3;

        public int CurrentPage { get; init; } = 1;

        public int TotalNewsArticles { get; set; }

        public string Category { get; init; }

        public string Tag { get; init; }

        [Display(Name = DisplaySearchByText)]
        public string SearchTerm { get; init; }

        [Display(Name = DisplaySortByDate)]
        public NewsSorting Sorting { get; init; }

        public IEnumerable<string> Categories { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public IEnumerable<NewsArticleViewModel> News { get; set; }
    }
}
