namespace TechExpoWorld.Web.ViewModels.News
{
    using TechExpoWorld.Data.Models;
    using TechExpoWorld.Services.Mapping;

    public class NewsArticleTitleViewModel : INewsArticleModel, IMapFrom<NewsArticle>
    {
        public string Title { get; init; }
    }
}
