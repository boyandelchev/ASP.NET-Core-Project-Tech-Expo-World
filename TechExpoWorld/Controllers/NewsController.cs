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
    using TechExpoWorld.Services.News;

    public class NewsController : Controller
    {
        private readonly INewsService news;
        private readonly TechExpoDbContext data;

        public NewsController(INewsService news, TechExpoDbContext data)
        {
            this.news = news;
            this.data = data;
        }

        [Authorize]
        public IActionResult Add()
        {
            if (!this.UserIsAuthor())
            {
                return RedirectToAction(nameof(AuthorsController.BecomeAuthor), "Authors");
            }

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

        public IActionResult All([FromQuery] AllNewsQueryModel query)
        {
            var newsQueryResult = this.news.All(
                query.Category,
                query.Tag,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllNewsQueryModel.NewsArticlesPerPage);

            var newsArticlesCategories = this.news.AllNewsCategories();

            var newsArticlesTags = this.news.AllNewsTags();

            query.Categories = newsArticlesCategories;
            query.Tags = newsArticlesTags;
            query.TotalNewsArticles = newsQueryResult.TotalNewsArticles;
            query.News = newsQueryResult.News;

            return View(query);
        }

        private bool UserIsAuthor()
            => this.data
                .Authors
                .Any(a => a.UserId == this.User.GetId());

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
