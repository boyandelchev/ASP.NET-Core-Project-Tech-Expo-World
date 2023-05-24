namespace TechExpoWorld.Web.ViewModels.Home
{
    using System.Collections.Generic;

    using TechExpoWorld.Web.ViewModels.News;
    using TechExpoWorld.Web.ViewModels.Statistics;

    public class IndexViewModel
    {
        public StatisticsViewModel Statistics { get; init; }

        public IList<LatestNewsArticleViewModel> LatestNewsArticles { get; init; }
    }
}
