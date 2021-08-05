namespace TechExpoWorld.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using TechExpoWorld.Models.Home;
    using TechExpoWorld.Services.News;
    using TechExpoWorld.Services.Statistics;

    public class HomeController : Controller
    {
        private readonly INewsService news;
        private readonly IStatisticsService statistics;

        public HomeController(INewsService news, IStatisticsService statistics)
        {
            this.news = news;
            this.statistics = statistics;
        }

        public IActionResult Index()
        {
            var newsArticles = this.news.LatestNewsArticles();

            var totalStatistics = this.statistics.Total();

            return View(new IndexViewModel
            {
                TotalNewsArticles = totalStatistics.TotalNewsArticles,
                TotalUsers = totalStatistics.TotalUsers,
                TotalAuthors = totalStatistics.TotalAuthors,
                TotalEvents = totalStatistics.TotalEvents,
                TotalLocations = totalStatistics.TotalLocations,
                News = newsArticles
            });
        }

        public IActionResult Error() => View();
    }
}
