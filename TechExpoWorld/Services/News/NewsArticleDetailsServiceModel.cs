namespace TechExpoWorld.Services.News
{
    using System.Collections.Generic;

    public class NewsArticleDetailsServiceModel : NewsArticleServiceModel
    {
        public int CategoryId { get; init; }

        public int AuthorId { get; init; }

        public IEnumerable<string> TagNames { get; init; }

        public IEnumerable<int> TagIds { get; init; }

        public string UserId { get; init; }
    }
}
