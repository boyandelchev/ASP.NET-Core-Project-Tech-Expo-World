namespace TechExpoWorld.Web.ViewModels.News
{
    using System.Collections.Generic;

    public class NewsArticlesQueryViewModel
    {
        public int TotalNewsArticles { get; init; }

        public IEnumerable<NewsArticleViewModel> News { get; init; }
    }
}
