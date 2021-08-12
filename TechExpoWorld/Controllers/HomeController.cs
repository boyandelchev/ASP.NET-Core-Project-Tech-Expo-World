namespace TechExpoWorld.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using TechExpoWorld.Models.Home;
    using TechExpoWorld.Services.News;
    using TechExpoWorld.Services.Statistics;

    public class HomeController : Controller
    {
        private readonly INewsService news;
        private readonly IStatisticsService statistics;
        private readonly IMapper mapper;

        public HomeController(INewsService news, IStatisticsService statistics, IMapper mapper)
        {
            this.news = news;
            this.statistics = statistics;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            var totalStatistics = this.statistics.Total();

            var indexData = this.mapper.Map<IndexViewModel>(totalStatistics);

            indexData.News = this.news.LatestNewsArticles();

            return View(indexData);
        }

        public IActionResult Error() => View();
    }
}
