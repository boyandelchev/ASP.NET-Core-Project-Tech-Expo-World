namespace TechExpoWorld.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using TechExpoWorld.Data;
    using TechExpoWorld.Models;
    using TechExpoWorld.Models.Home;
    using TechExpoWorld.Services.Statistics;

    public class HomeController : Controller
    {
        private readonly IStatisticsService statistics;
        private readonly TechExpoDbContext data;

        public HomeController(IStatisticsService statistics, TechExpoDbContext data)
        {
            this.statistics = statistics;
            this.data = data;
        }

        public IActionResult Index()
        {
            var newsArticles = this.data
                .NewsArticles
                .OrderByDescending(c => c.Id)
                .Select(na => new NewsArticleIndexViewModel
                {
                    Id = na.Id,
                    Title = na.Title,
                    ImageUrl = na.ImageUrl
                })
                .Take(3)
                .ToList();

            var totalStatistics = this.statistics.Total();

            return View(new IndexViewModel
            {
                TotalNewsArticles = totalStatistics.TotalNewsArticles,
                TotalUsers = totalStatistics.TotalUsers,
                TotalAuthors = totalStatistics.TotalAuthors,
                News = newsArticles
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
