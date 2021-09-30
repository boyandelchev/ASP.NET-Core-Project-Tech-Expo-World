namespace TechExpoWorld.Services.News.Models
{
    public class LatestNewsArticleServiceModel : INewsArticleModel
    {
        public int Id { get; init; }

        public string Title { get; init; }

        public string ImageUrl { get; init; }
    }
}
