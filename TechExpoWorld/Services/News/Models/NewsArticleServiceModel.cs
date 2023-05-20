namespace TechExpoWorld.Services.News.Models
{
    public class NewsArticleServiceModel : INewsArticleModel
    {
        public int Id { get; init; }

        public string Title { get; init; }

        public string Content { get; init; }

        public string ImageUrl { get; init; }

        public string CreatedOn { get; init; }

        public string AuthorName { get; init; }

        public string CategoryName { get; init; }
    }
}
