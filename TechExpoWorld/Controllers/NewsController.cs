namespace TechExpoWorld.Controllers
{
    using System.Collections.Generic;
    using System.Globalization;
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
                NewsCategories = this.GetNewsCategories(),
                Tags = this.GetTags()
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

            if (!this.data.NewsCategories.Any(nc => nc.Id == news.NewsCategoryId))
            {
                this.ModelState.AddModelError(nameof(news.NewsCategoryId), "News category does not exist.");
            }

            if (!news.TagIds.All(tId => this.data.Tags.Select(t => t.Id).Contains(tId)))
            {
                this.ModelState.AddModelError(nameof(news.TagIds), "Tag option does not exist.");
            }

            if (!ModelState.IsValid)
            {
                news.NewsCategories = this.GetNewsCategories();
                news.Tags = this.GetTags();

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

            if (news.TagIds.Any())
            {
                var newsArticleTags = new List<NewsArticleTag>();

                foreach (var tagId in news.TagIds)
                {
                    newsArticleTags.Add(new NewsArticleTag { TagId = tagId });
                }

                newsData.NewsArticleTags = newsArticleTags;
            }

            this.data.NewsArticles.Add(newsData);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        public IActionResult All(List<AllNewsQueryModel> allNews)
        {
            allNews = this.data
               .NewsArticles
               .Select(na => new AllNewsQueryModel
               {
                   Id = na.Id,
                   Title = na.Title,
                   Content = na.Content.Substring(0, 200) + "...",
                   ImageUrl = na.ImageUrl,
                   CreatedOn = na.CreatedOn.ToString("dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture),
                   NewsCategory = na.NewsCategory.Name,
                   Author = na.Author.Name
               })
               .ToList();

            return View(allNews);
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

        private IEnumerable<TagViewModel> GetTags()
            => this.data
                .Tags
                .Select(t => new TagViewModel
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .ToList();
    }
}
