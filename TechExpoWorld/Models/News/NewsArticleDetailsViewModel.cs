namespace TechExpoWorld.Models.News
{
    using System.Collections.Generic;

    public class NewsArticleDetailsViewModel
    {
        public string Title { get; init; }

        public string Content { get; init; }

        public string ImageUrl { get; init; }

        public string CreatedOn { get; init; }

        public string LastModifiedOn { get; init; }

        public int ViewCount { get; init; }

        public string AuthorName { get; init; }

        public string CategoryName { get; init; }

        public IEnumerable<string> TagNames { get; init; }
    }
}
