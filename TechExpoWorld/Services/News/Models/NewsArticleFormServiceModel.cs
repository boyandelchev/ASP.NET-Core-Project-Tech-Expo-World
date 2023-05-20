namespace TechExpoWorld.Services.News.Models
{
    using System.Collections.Generic;

    public class NewsArticleFormServiceModel
    {
        public string Title { get; init; }

        public string Content { get; init; }

        public string ImageUrl { get; init; }

        public string AuthorId { get; init; }

        public int CategoryId { get; init; }

        public IEnumerable<int> TagIds { get; init; }
    }
}
