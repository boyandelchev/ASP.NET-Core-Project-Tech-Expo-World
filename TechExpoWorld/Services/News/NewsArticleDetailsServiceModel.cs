namespace TechExpoWorld.Services.News
{
    using System.Collections.Generic;

    public class NewsArticleDetailsServiceModel : NewsArticleServiceModel
    {
        public string LastModifiedOn { get; init; }

        public int ViewCount { get; init; }

        public int CategoryId { get; init; }

        public int AuthorId { get; init; }

        public IEnumerable<int> TagIds { get; init; }

        public IEnumerable<string> TagNames { get; init; }

        public string UserId { get; init; }
    }
}
