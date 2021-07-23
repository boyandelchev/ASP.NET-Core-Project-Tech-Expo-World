namespace TechExpoWorld.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using TechExpoWorld.Data;
    using TechExpoWorld.Models;
    using TechExpoWorld.Models.Home;

    public class HomeController : Controller
    {
        private readonly TechExpoDbContext data;

        public HomeController(TechExpoDbContext data)
            => this.data = data;

        public IActionResult Index()
        {
            var totalNewsArticles = this.data.NewsArticles.Count();

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

            return View(new IndexViewModel
            {
                TotalNewsArticles = totalNewsArticles,
                News = newsArticles
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
