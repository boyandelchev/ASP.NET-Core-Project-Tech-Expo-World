namespace TechExpoWorld.Services.News
{
    using System.Collections.Generic;

    public class NewsArticlesQueryServiceModel
    {
        public int TotalNewsArticles { get; init; }

        public IEnumerable<NewsArticleServiceModel> News { get; init; }
    }
}
