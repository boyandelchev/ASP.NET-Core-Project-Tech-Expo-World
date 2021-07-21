namespace TechExpoWorld.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TechExpoWorld.Data;
    using TechExpoWorld.Data.Models;
    using TechExpoWorld.Infrastructure;
    using TechExpoWorld.Models.News;

    public class NewsController : Controller
    {
        private readonly TechExpoDbContext data;

        public NewsController(TechExpoDbContext data)
            => this.data = data;

        [Authorize]
        public IActionResult Add()
        {
            return View(new AddNewsFormModel
            {
                NewsCategories = GetNewsCategories()
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddNewsFormModel news)
        {
            var authorId = this.data
                .Authors
                .Where(a => a.UserId == this.User.GetId())
                .Select(a => a.Id)
                .FirstOrDefault();

            if (authorId == 0)
            {
                return RedirectToAction(nameof(AuthorsController.BecomeAuthor), "Authors");
            }

            if (!this.data.NewsCategories.Any(c => c.Id == news.NewsCategoryId))
            {
                this.ModelState.AddModelError(nameof(news.NewsCategoryId), "News category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                news.NewsCategories = this.GetNewsCategories();

                return View(news);
            }

            var newsData = new NewsArticle
            {
                Title = news.Title,
                Content = news.Content,
                ImageUrl = news.ImageUrl,
                NewsCategoryId = news.NewsCategoryId,
                AuthorId = authorId
            };

            this.data.NewsArticles.Add(newsData);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        public IActionResult All()
        {
            return View();
        }

        private IEnumerable<NewsCategoryViewModel> GetNewsCategories()
            => this.data
                .NewsCategories
                .Select(nc => new NewsCategoryViewModel
                {
                    Id = nc.Id,
                    Name = nc.Name
                })
                .ToList();
    }
}
