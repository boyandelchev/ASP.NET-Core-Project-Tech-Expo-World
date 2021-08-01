namespace TechExpoWorld.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TechExpoWorld.Infrastructure;
    using TechExpoWorld.Models.News;
    using TechExpoWorld.Services.Authors;
    using TechExpoWorld.Services.News;

    public class NewsController : Controller
    {
        private readonly INewsService news;
        private readonly IAuthorService authors;

        public NewsController(INewsService news, IAuthorService authors)
        {
            this.news = news;
            this.authors = authors;
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

            var newsArticlesCategories = this.news.CategoryNames();
            var newsArticlesTags = this.news.TagNames();

            query.Categories = newsArticlesCategories;
            query.Tags = newsArticlesTags;
            query.TotalNewsArticles = newsQueryResult.TotalNewsArticles;
            query.News = newsQueryResult.News;

            return View(query);
        }

        [Authorize]
        public IActionResult Add()
        {
            if (!this.authors.IsAuthor(this.User.Id()))
            {
                return RedirectToAction(nameof(AuthorsController.BecomeAuthor), "Authors");
            }

            return View(new AddNewsFormModel
            {
                Categories = this.news.Categories(),
                Tags = this.news.Tags()
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddNewsFormModel newsArticle)
        {
            var authorId = this.authors.AuthorId(this.User.Id());

            if (authorId == 0)
            {
                return RedirectToAction(nameof(AuthorsController.BecomeAuthor), "Authors");
            }

            if (!this.news.CategoryExists(newsArticle.NewsCategoryId))
            {
                this.ModelState.AddModelError(nameof(newsArticle.NewsCategoryId), "News category does not exist.");
            }

            if (!newsArticle.TagIds.All(tagId => this.news.TagExists(tagId)))
            {
                this.ModelState.AddModelError(nameof(newsArticle.TagIds), "Tag option does not exist.");
            }

            if (!ModelState.IsValid)
            {
                newsArticle.Categories = this.news.Categories();
                newsArticle.Tags = this.news.Tags();

                return View(newsArticle);
            }

            this.news.Create(
                newsArticle.Title,
                newsArticle.Content,
                newsArticle.ImageUrl,
                newsArticle.NewsCategoryId,
                authorId,
                newsArticle.TagIds);

            return RedirectToAction(nameof(All));
        }
    }
}
