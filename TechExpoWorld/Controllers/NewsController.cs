namespace TechExpoWorld.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using TechExpoWorld.Infrastructure.Extensions;
    using TechExpoWorld.Models.Comments;
    using TechExpoWorld.Models.News;
    using TechExpoWorld.Services.Authors;
    using TechExpoWorld.Services.Comments;
    using TechExpoWorld.Services.News;

    using static GlobalConstants.NewsArticle;
    using static GlobalConstants.TempData;

    public class NewsController : Controller
    {
        private const string ControllerAuthors = "Authors";
        private readonly INewsService news;
        private readonly IAuthorsService authors;
        private readonly ICommentsService comments;
        private readonly IMapper mapper;

        public NewsController(
            INewsService news,
            IAuthorsService authors,
            ICommentsService comments,
            IMapper mapper)
        {
            this.news = news;
            this.authors = authors;
            this.comments = comments;
            this.mapper = mapper;
        }

        public async Task<IActionResult> All([FromQuery] AllNewsQueryModel query)
        {
            var newsQueryResult = await this.news.AllAsync(
                query.Category,
                query.Tag,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllNewsQueryModel.NewsArticlesPerPage);

            query.Categories = await this.news.CategoryNamesAsync();
            query.Tags = await this.news.TagNamesAsync();
            query.TotalNewsArticles = newsQueryResult.TotalNewsArticles;
            query.News = newsQueryResult.News;

            return View(query);
        }

        public async Task<IActionResult> Details(int id, string information)
        {
            var newsArticle = await this.news.DetailsAsync(id);

            if (newsArticle == null)
            {
                return NotFound();
            }

            if (information != newsArticle.GetNewsArticleInformation())
            {
                return BadRequest();
            }

            var comments = await this.comments.CommentsOnNewsArticleAsync(id);
            var totalComments = await this.comments.TotalCommentsOnNewsArticleAsync(id);

            return View(new NewsArticleWithCommentsViewModel
            {
                NewsArticle = newsArticle,
                CommentForm = new CommentFormModel(),
                Comments = comments,
                TotalComments = totalComments
            });
        }

        [Authorize]
        public async Task<IActionResult> MyNewsArticles()
        {
            if (this.User.IsAdmin())
            {
                return BadRequest();
            }

            var authorId = await this.authors.AuthorIdAsync(this.User.Id());

            var myNewsArticles = await this.news.NewsArticlesByAuthorAsync(authorId);

            return View(myNewsArticles);
        }

        [Authorize]
        public async Task<IActionResult> Add()
        {
            if (this.User.IsAdmin())
            {
                return BadRequest();
            }

            if (!await this.authors.IsAuthorAsync(this.User.Id()))
            {
                return RedirectToAction(nameof(AuthorsController.BecomeAuthor), ControllerAuthors);
            }

            return View(new NewsArticleFormModel
            {
                Categories = await this.news.CategoriesAsync(),
                Tags = await this.news.TagsAsync()
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(NewsArticleFormModel newsArticle)
        {
            if (this.User.IsAdmin())
            {
                return BadRequest();
            }

            var authorId = await this.authors.AuthorIdAsync(this.User.Id());

            if (authorId == null)
            {
                return RedirectToAction(nameof(AuthorsController.BecomeAuthor), ControllerAuthors);
            }

            if (!await this.news.CategoryExistsAsync(newsArticle.CategoryId))
            {
                this.ModelState.AddModelError(nameof(newsArticle.CategoryId), ErrorCategory);
            }

            if (!this.news.TagsExist(newsArticle.TagIds))
            {
                this.ModelState.AddModelError(nameof(newsArticle.TagIds), ErrorTag);
            }

            if (!ModelState.IsValid)
            {
                newsArticle.Categories = await this.news.CategoriesAsync();
                newsArticle.Tags = await this.news.TagsAsync();

                return View(newsArticle);
            }

            var newsArticleId = await this.news.CreateAsync(
                newsArticle.Title,
                newsArticle.Content,
                newsArticle.ImageUrl,
                newsArticle.CategoryId,
                newsArticle.TagIds,
                authorId);

            TempData[GlobalMessageKey] = CreatedNewsArticle;

            return RedirectToAction(
                nameof(Details),
                new { id = newsArticleId, information = newsArticle.GetNewsArticleInformation() });
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var authorId = await this.authors.AuthorIdAsync(this.User.Id());

            if (authorId == null && !this.User.IsAdmin())
            {
                return RedirectToAction(nameof(AuthorsController.BecomeAuthor), ControllerAuthors);
            }

            var newsArticle = await this.news.NewsArticleFormData(id);

            if (newsArticle == null)
            {
                return NotFound();
            }

            if (newsArticle.AuthorId != authorId && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            var newsArticleData = this.mapper.Map<NewsArticleFormModel>(newsArticle);

            newsArticleData.Categories = await this.news.CategoriesAsync();
            newsArticleData.Tags = await this.news.TagsAsync();

            return View(newsArticleData);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(int id, NewsArticleFormModel newsArticle)
        {
            var authorId = await this.authors.AuthorIdAsync(this.User.Id());

            if (authorId == null && !this.User.IsAdmin())
            {
                return RedirectToAction(nameof(AuthorsController.BecomeAuthor), ControllerAuthors);
            }

            if (!await this.news.IsByAuthorAsync(id, authorId) && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            if (!await this.news.CategoryExistsAsync(newsArticle.CategoryId))
            {
                this.ModelState.AddModelError(nameof(newsArticle.CategoryId), ErrorCategory);
            }

            if (!this.news.TagsExist(newsArticle.TagIds))
            {
                this.ModelState.AddModelError(nameof(newsArticle.TagIds), ErrorTag);
            }

            if (!ModelState.IsValid)
            {
                newsArticle.Categories = await this.news.CategoriesAsync();
                newsArticle.Tags = await this.news.TagsAsync();

                return View(newsArticle);
            }

            await this.news.EditAsync(
                id,
                newsArticle.Title,
                newsArticle.Content,
                newsArticle.ImageUrl,
                newsArticle.CategoryId,
                newsArticle.TagIds);

            TempData[GlobalMessageKey] = EditedNewsArticle;

            return RedirectToAction(
                nameof(Details),
                new { id, information = newsArticle.GetNewsArticleInformation() });
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id) => await Edit(id);

        [ActionName(nameof(Delete))]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var authorId = await this.authors.AuthorIdAsync(this.User.Id());

            if (authorId == null && !this.User.IsAdmin())
            {
                return RedirectToAction(nameof(AuthorsController.BecomeAuthor), ControllerAuthors);
            }

            if (!await this.news.IsByAuthorAsync(id, authorId) && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            await this.news.DeleteAsync(id);

            TempData[GlobalMessageKey] = DeletedNewsArticle;

            return RedirectToAction(nameof(All));
        }
    }
}
