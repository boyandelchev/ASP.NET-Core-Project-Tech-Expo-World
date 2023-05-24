namespace TechExpoWorld.Web.ViewModels.News
{
    public class LatestNewsArticleViewModel : INewsArticleModel
    {
        public int Id { get; init; }

        public string Title { get; init; }

        public string ImageUrl { get; init; }
    }
}
