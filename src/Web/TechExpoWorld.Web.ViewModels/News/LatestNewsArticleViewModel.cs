namespace TechExpoWorld.Web.ViewModels.News
{
    using TechExpoWorld.Data.Models;
    using TechExpoWorld.Services.Mapping;

    public class LatestNewsArticleViewModel : INewsArticleModel, IMapFrom<NewsArticle>
    {
        public int Id { get; init; }

        public string Title { get; init; }

        public string ImageUrl { get; init; }
    }
}
