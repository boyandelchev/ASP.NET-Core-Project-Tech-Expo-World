namespace TechExpoWorld.Web.ViewModels.News
{
    using System.Collections.Generic;

    public class NewsArticleDetailsViewModel : NewsArticleViewModel
    {
        public string LastModifiedOn { get; init; }

        public int ViewCount { get; init; }

        public IEnumerable<string> TagNames { get; init; }
    }
}
