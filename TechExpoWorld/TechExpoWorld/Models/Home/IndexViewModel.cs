namespace TechExpoWorld.Models.Home
{
    using System.Collections.Generic;

    using TechExpoWorld.Services.News.Models;
    using TechExpoWorld.Services.Statistics.Models;

    public class IndexViewModel
    {
        public StatisticsServiceModel Statistics { get; init; }

        public IList<LatestNewsArticleServiceModel> LatestNewsArticles { get; init; }
    }
}
