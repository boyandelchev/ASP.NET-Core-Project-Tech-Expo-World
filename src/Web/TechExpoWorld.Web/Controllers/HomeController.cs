namespace TechExpoWorld.Web.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    using TechExpoWorld.Services.Data.News;
    using TechExpoWorld.Services.Data.Statistics;
    using TechExpoWorld.Web.ViewModels;
    using TechExpoWorld.Web.ViewModels.Home;
    using TechExpoWorld.Web.ViewModels.News;
    using TechExpoWorld.Web.ViewModels.Statistics;

    using static TechExpoWorld.Common.GlobalConstants.Cache;

    public class HomeController : BaseController
    {
        private readonly INewsService newsService;
        private readonly IStatisticsService statisticsService;
        private readonly IMemoryCache cache;

        public HomeController(
            INewsService newsService,
            IStatisticsService statisticsService,
            IMemoryCache cache)
        {
            this.newsService = newsService;
            this.statisticsService = statisticsService;
            this.cache = cache;
        }

        public async Task<IActionResult> Index()
        {
            var indexData = this.cache.Get<IndexViewModel>(LatestStatisticsAndNewsArticlesCacheKey);

            if (indexData == null)
            {
                indexData = new IndexViewModel
                {
                    Statistics = new StatisticsViewModel
                    {
                        TotalNewsArticles = await this.statisticsService.TotalNewsArticlesAsync(),
                        TotalUsers = await this.statisticsService.TotalUsersAsync(),
                        TotalAuthors = await this.statisticsService.TotalAuthorsAsync(),
                        TotalAttendees = await this.statisticsService.TotalAttendeesAsync(),
                        TotalEvents = await this.statisticsService.TotalEventsAsync(),
                        TotalLocations = await this.statisticsService.TotalLocationsAsync(),
                    },
                    LatestNewsArticles = await this.newsService.LatestNewsArticlesAsync<LatestNewsArticleViewModel>(),
                };

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(1));

                this.cache.Set(LatestStatisticsAndNewsArticlesCacheKey, indexData, cacheOptions);
            }

            return this.View(indexData);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
