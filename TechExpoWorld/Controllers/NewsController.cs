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

    using static WebConstants;

    public class NewsController : Controller
    {
        private readonly INewsService news;
        private readonly IAuthorService authors;
        private readonly ICommentService comments;
        private readonly IMapper mapper;

        public NewsController(
            INewsService news,
            IAuthorService authors,
            ICommentService comments,
            IMapper mapper)
        {
            this.news = news;
            this.authors = authors;
            this.comments = comments;
            this.mapper = mapper;
        }

        public async Task<IActionResult> All([FromQuery] AllNewsQueryModel query)
        {
            var newsQueryResult = await this.news.All(
                query.Category,
                query.Tag,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllNewsQueryModel.NewsArticlesPerPage);

            query.Categories = await this.news.CategoryNames();
            query.Tags = await this.news.TagNames();
            query.TotalNewsArticles = newsQueryResult.TotalNewsArticles;
            query.News = newsQueryResult.News;

            return View(query);
        }

        public async Task<IActionResult> Details(int id, string information)
        {
            var newsArticle = await this.news.Details(id);

            if (newsArticle == null)
            {
                return NotFound();
            }

            if (information != newsArticle.GetNewsArticleInformation())
            {
                return BadRequest();
            }

            var newsArticleData = this.mapper.Map<NewsArticleDetailsViewModel>(newsArticle);

            var comments = await this.comments.CommentsOnNewsArticle(id);
            var totalComments = await this.comments.TotalCommentsOnNewsArticle(id);

            return View(new NewsArticleWithCommentsViewModel
            {
                NewsArticleId = id,
                NewsArticle = newsArticleData,
                Comment = new CommentFormModel(),
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

            var myNewsArticles = await this.news.NewsArticlesByUser(this.User.Id());

            return View(myNewsArticles);
        }

        [Authorize]
        public async Task<IActionResult> Add()
        {
            if (this.User.IsAdmin())
            {
                return BadRequest();
            }

            if (!await this.authors.IsAuthor(this.User.Id()))
            {
                return RedirectToAction(nameof(AuthorsController.BecomeAuthor), "Authors");
            }

            return View(new NewsArticleFormModel
            {
                Categories = await this.news.Categories(),
                Tags = await this.news.Tags()
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

            var authorId = await this.authors.AuthorId(this.User.Id());

            if (authorId == 0)
            {
                return RedirectToAction(nameof(AuthorsController.BecomeAuthor), "Authors");
            }

            if (!await this.news.CategoryExists(newsArticle.CategoryId))
            {
                this.ModelState.AddModelError(nameof(newsArticle.CategoryId), "News category does not exist.");
            }

            if (!this.news.TagsExist(newsArticle.TagIds))
            {
                this.ModelState.AddModelError(nameof(newsArticle.TagIds), "Tag option does not exist.");
            }

            if (!ModelState.IsValid)
            {
                newsArticle.Categories = await this.news.Categories();
                newsArticle.Tags = await this.news.Tags();

                return View(newsArticle);
            }

            var newsArticleId = await this.news.Create(
                newsArticle.Title,
                newsArticle.Content,
                newsArticle.ImageUrl,
                newsArticle.CategoryId,
                newsArticle.TagIds,
                authorId);

            TempData[GlobalMessageKey] = "Your news article was added successfully!";

            return RedirectToAction(nameof(Details), new { id = newsArticleId, information = newsArticle.GetNewsArticleInformation() });
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = this.User.Id();

            if (!await this.authors.IsAuthor(userId) && !this.User.IsAdmin())
            {
                return RedirectToAction(nameof(AuthorsController.BecomeAuthor), "Authors");
            }

            var newsArticle = await this.news.DetailsWithNoViewCountIncrement(id);

            if (newsArticle == null)
            {
                return NotFound();
            }

            if (newsArticle.UserId != userId && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            var newsArticleForm = this.mapper.Map<NewsArticleFormModel>(newsArticle);

            newsArticleForm.Categories = await this.news.Categories();
            newsArticleForm.Tags = await this.news.Tags();

            return View(newsArticleForm);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(int id, NewsArticleFormModel newsArticle)
        {
            var authorId = await this.authors.AuthorId(this.User.Id());

            if (authorId == 0 && !this.User.IsAdmin())
            {
                return RedirectToAction(nameof(AuthorsController.BecomeAuthor), "Authors");
            }

            if (!await this.news.IsByAuthor(id, authorId) && !this.User.IsAdmin())
            {
                return BadRequest();
            }

            if (!await this.news.CategoryExists(newsArticle.CategoryId))
            {
                this.ModelState.AddModelError(nameof(newsArticle.CategoryId), "News category does not exist.");
            }

            if (!this.news.TagsExist(newsArticle.TagIds))
            {
                this.ModelState.AddModelError(nameof(newsArticle.TagIds), "Tag option does not exist.");
            }

            if (!ModelState.IsValid)
            {
                newsArticle.Categories = await this.news.Categories();
                newsArticle.Tags = await this.news.Tags();

                return View(newsArticle);
            }

            await this.news.Edit(
                 id,
                 newsArticle.Title,
                 newsArticle.Content,
                 newsArticle.ImageUrl,
                 newsArticle.CategoryId,
                 newsArticle.TagIds);

            TempData[GlobalMessageKey] = "Your news article was edited successfully!";

            return RedirectToAction(nameof(Details), new { id, information = newsArticle.GetNewsArticleInformation() });
        }

        [Authorize]
        public async Task<IActionResult> DeleteDetails(int id)
        {
            var userId = this.User.Id();

            if (!await this.authors.IsAuthor(userId) && !this.User.IsAdmin())
            {
                return RedirectToAction(nameof(AuthorsController.BecomeAuthor), "Authors");
            }

            var newsArticle = await this.news.DetailsWithNoViewCountIncrement(id);

            if (newsArticle == null)
            {
                return NotFound();
            }

            if (newsArticle.UserId != userId && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            var newsArticleData = this.mapper.Map<NewsArticleDeleteDetailsViewModel>(newsArticle);

            newsArticleData.Categories = await this.news.Categories();
            newsArticleData.Tags = await this.news.Tags();

            return View(newsArticleData);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var authorId = await this.authors.AuthorId(this.User.Id());

            if (authorId == 0 && !this.User.IsAdmin())
            {
                return RedirectToAction(nameof(AuthorsController.BecomeAuthor), "Authors");
            }

            if (!await this.news.IsByAuthor(id, authorId) && !this.User.IsAdmin())
            {
                return BadRequest();
            }

            await this.news.Delete(id);

            TempData[GlobalMessageKey] = "Your news article was deleted successfully!";

            return RedirectToAction(nameof(All));
        }
    }
}
