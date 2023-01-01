namespace TechExpoWorld.Test.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TechExpoWorld.Data.Models;

    public static class NewsArticles
    {
        public static IEnumerable<NewsArticle> TenNewsArticles
            => Enumerable.Range(0, 10).Select(i => new NewsArticle
            {
                Title = "Article"
            });

        public static NewsArticle OneNewsArticle
            => new NewsArticle
            {
                Id = 1,
                Title = "Article",
                Content = "content",
                ImageUrl = "https://imageUrl.jpg",
                CreatedOn = DateTime.UtcNow,
                LastModifiedOn = DateTime.UtcNow,
                ViewCount = 0,
                NewsCategoryId = 1,
                NewsCategory = new NewsCategory { Id = 1, Name = "AI" },
                AuthorId = "1",
                Author = new Author { Id = "1" }
            };
    }
}
