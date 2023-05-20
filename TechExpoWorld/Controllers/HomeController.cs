namespace TechExpoWorld.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    using TechExpoWorld.Models.Home;
    using TechExpoWorld.Services.News;
    using TechExpoWorld.Services.Statistics;

    using static GlobalConstants.Cache;

    public class HomeController : Controller
    {
        private readonly INewsService news;
        private readonly IStatisticsService statistics;
        private readonly IMemoryCache cache;

        public HomeController(
            INewsService news,
            IStatisticsService statistics,
            IMemoryCache cache)
        {
            this.news = news;
            this.statistics = statistics;
            this.cache = cache;
        }

        public async Task<IActionResult> Index()
        {
            var indexData = this.cache.Get<IndexViewModel>(LatestStatisticsAndNewsArticlesCacheKey);

            if (indexData == null)
            {
                indexData = new IndexViewModel
                {
                    Statistics = await this.statistics.TotalAsync(),
                    LatestNewsArticles = await this.news.LatestNewsArticlesAsync()
                };

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(1));

                this.cache.Set(LatestStatisticsAndNewsArticlesCacheKey, indexData, cacheOptions);
            }

            return View(indexData);
        }

        public IActionResult Error() => View();
    }
}
