﻿namespace TechExpoWorld.Controllers
{
    using System;
    using System.Threading.Tasks;

    using AutoMapper;

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
        private readonly IMapper mapper;
        private readonly IMemoryCache cache;

        public HomeController(
            INewsService news,
            IStatisticsService statistics,
            IMapper mapper,
            IMemoryCache cache)
        {
            this.news = news;
            this.statistics = statistics;
            this.mapper = mapper;
            this.cache = cache;
        }

        public async Task<IActionResult> Index()
        {
            var indexData = this.cache.Get<IndexViewModel>(LatestStatisticsAndNewsArticlesCacheKey);

            if (indexData == null)
            {
                var totalStatistics = await this.statistics.TotalAsync();

                indexData = this.mapper.Map<IndexViewModel>(totalStatistics);

                indexData.News = await this.news.LatestNewsArticlesAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(1));

                this.cache.Set(LatestStatisticsAndNewsArticlesCacheKey, indexData, cacheOptions);
            }

            return View(indexData);
        }

        public IActionResult Error() => View();
    }
}
