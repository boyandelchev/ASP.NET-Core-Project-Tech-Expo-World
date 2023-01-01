namespace TechExpoWorld.Services.News.Models
{
    using System.Collections.Generic;

    public class NewsArticleDetailsServiceModel : NewsArticleServiceModel
    {
        public string LastModifiedOn { get; init; }

        public int ViewCount { get; init; }

        public int CategoryId { get; init; }

        public string AuthorId { get; init; }

        public IEnumerable<int> TagIds { get; init; }

        public IEnumerable<string> TagNames { get; init; }
    }
}
