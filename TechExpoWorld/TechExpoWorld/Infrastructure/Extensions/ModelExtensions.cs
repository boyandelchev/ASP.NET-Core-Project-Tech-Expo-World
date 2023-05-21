namespace TechExpoWorld.Infrastructure.Extensions
{
    using TechExpoWorld.Services.Events.Models;
    using TechExpoWorld.Services.News.Models;

    public static class ModelExtensions
    {
        public static string GetNewsArticleInformation(this INewsArticleModel newsArticle)
            => newsArticle.Title.Replace(" ", "-");

        public static string GetEventInformation(this IEventModel @event)
            => @event.Title.Replace(" ", "-");
    }
}
