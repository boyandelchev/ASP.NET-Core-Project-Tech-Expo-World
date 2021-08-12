namespace TechExpoWorld.Services.News.Models
{
    using System.Collections.Generic;

    public class NewsArticlesQueryServiceModel
    {
        public int TotalNewsArticles { get; init; }

        public IEnumerable<NewsArticleServiceModel> News { get; init; }
    }
}
