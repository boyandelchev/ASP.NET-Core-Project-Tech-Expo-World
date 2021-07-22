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
            var newsQuery = this.data.NewsArticles.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Category))
            {
                newsQuery = newsQuery
                    .Where(na => na.NewsCategory.Name == query.Category);
            }

            if (!string.IsNullOrWhiteSpace(query.Tag))
            {
                newsQuery = newsQuery
                    .Where(na => na.NewsArticleTags
                        .Select(nat => nat.Tag.Name)
                        .Contains(query.Tag));
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                newsQuery = newsQuery.Where(na =>
                    na.Author.Name.ToLower().Contains(query.SearchTerm.ToLower()) ||
                    na.Title.ToLower().Contains(query.SearchTerm.ToLower()) ||
                    na.Content.ToLower().Contains(query.SearchTerm.ToLower()));
            }

            newsQuery = query.Sorting switch
            {
                NewsSorting.Ascending => newsQuery.OrderBy(na => na.Id),
                NewsSorting.Descending or _ => newsQuery.OrderByDescending(na => na.Id)
            };

            var totalNewsArticles = newsQuery.Count();

            var news = newsQuery
                .Skip((query.CurrentPage - 1) * AllNewsQueryModel.NewsArticlesPerPage)
                .Take(AllNewsQueryModel.NewsArticlesPerPage)
                .Select(na => new NewsArticleListingViewModel
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

            var categories = this.data
                .NewsCategories
                .Select(nc => nc.Name)
                .OrderBy(name => name)
                .ToList();

            var tags = this.data
                .Tags
                .Select(t => t.Name.ToLower())
                .OrderBy(name => name)
                .ToList();

            query.TotalNewsArticles = totalNewsArticles;
            query.News = news;
            query.Categories = categories;
            query.Tags = tags;

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
