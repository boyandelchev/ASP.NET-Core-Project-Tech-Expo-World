namespace TechExpoWorld.Web.Infrastructure.Extensions
{
    using TechExpoWorld.Web.ViewModels.Events;
    using TechExpoWorld.Web.ViewModels.News;

    public static class ModelExtensions
    {
        public static string GetNewsArticleInformation(this INewsArticleModel newsArticle)
            => newsArticle.Title.Replace(" ", "-");

        public static string GetEventInformation(this IEventModel @event)
            => @event.Title.Replace(" ", "-");
    }
}
