namespace TechExpoWorld.Controllers
{
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

        public IActionResult All([FromQuery] AllNewsQueryModel query)
        {
            var newsQueryResult = this.news.All(
                query.Category,
                query.Tag,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllNewsQueryModel.NewsArticlesPerPage);

            query.Categories = this.news.CategoryNames();
            query.Tags = this.news.TagNames();
            query.TotalNewsArticles = newsQueryResult.TotalNewsArticles;
            query.News = newsQueryResult.News;

            return View(query);
        }

        public IActionResult Details(int id, string information)
        {
            var newsArticle = this.news.Details(id);

            if (newsArticle == null)
            {
                return NotFound();
            }

            if (information != newsArticle.GetNewsArticleInformation())
            {
                return BadRequest();
            }

            var newsArticleData = this.mapper.Map<NewsArticleDetailsViewModel>(newsArticle);

            var comments = this.comments.CommentsOnNewsArticle(id);
            var totalComments = this.comments.TotalCommentsOnNewsArticle(id);

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
        public IActionResult MyNewsArticles()
        {
            if (this.User.IsAdmin())
            {
                return BadRequest();
            }

            var myNewsArticles = this.news.NewsArticlesByUser(this.User.Id());

            return View(myNewsArticles);
        }

        [Authorize]
        public IActionResult Add()
        {
            if (this.User.IsAdmin())
            {
                return BadRequest();
            }

            if (!this.authors.IsAuthor(this.User.Id()))
            {
                return RedirectToAction(nameof(AuthorsController.BecomeAuthor), "Authors");
            }

            return View(new NewsArticleFormModel
            {
                Categories = this.news.Categories(),
                Tags = this.news.Tags()
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(NewsArticleFormModel newsArticle)
        {
            if (this.User.IsAdmin())
            {
                return BadRequest();
            }

            var authorId = this.authors.AuthorId(this.User.Id());

            if (authorId == 0)
            {
                return RedirectToAction(nameof(AuthorsController.BecomeAuthor), "Authors");
            }

            if (!this.news.CategoryExists(newsArticle.CategoryId))
            {
                this.ModelState.AddModelError(nameof(newsArticle.CategoryId), "News category does not exist.");
            }

            if (!this.news.TagsExist(newsArticle.TagIds))
            {
                this.ModelState.AddModelError(nameof(newsArticle.TagIds), "Tag option does not exist.");
            }

            if (!ModelState.IsValid)
            {
                newsArticle.Categories = this.news.Categories();
                newsArticle.Tags = this.news.Tags();

                return View(newsArticle);
            }

            var newsArticleId = this.news.Create(
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
        public IActionResult Edit(int id)
        {
            var userId = this.User.Id();

            if (!this.authors.IsAuthor(userId) && !this.User.IsAdmin())
            {
                return RedirectToAction(nameof(AuthorsController.BecomeAuthor), "Authors");
            }

            var newsArticle = this.news.DetailsWithNoViewCountIncrement(id);

            if (newsArticle == null)
            {
                return NotFound();
            }

            if (newsArticle.UserId != userId && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            var newsArticleForm = this.mapper.Map<NewsArticleFormModel>(newsArticle);

            newsArticleForm.Categories = this.news.Categories();
            newsArticleForm.Tags = this.news.Tags();

            return View(newsArticleForm);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(int id, NewsArticleFormModel newsArticle)
        {
            var authorId = this.authors.AuthorId(this.User.Id());

            if (authorId == 0 && !this.User.IsAdmin())
            {
                return RedirectToAction(nameof(AuthorsController.BecomeAuthor), "Authors");
            }

            if (!this.news.IsByAuthor(id, authorId) && !this.User.IsAdmin())
            {
                return BadRequest();
            }

            if (!this.news.CategoryExists(newsArticle.CategoryId))
            {
                this.ModelState.AddModelError(nameof(newsArticle.CategoryId), "News category does not exist.");
            }

            if (!this.news.TagsExist(newsArticle.TagIds))
            {
                this.ModelState.AddModelError(nameof(newsArticle.TagIds), "Tag option does not exist.");
            }

            if (!ModelState.IsValid)
            {
                newsArticle.Categories = this.news.Categories();
                newsArticle.Tags = this.news.Tags();

                return View(newsArticle);
            }

            this.news.Edit(
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
        public IActionResult DeleteDetails(int id)
        {
            var userId = this.User.Id();

            if (!this.authors.IsAuthor(userId) && !this.User.IsAdmin())
            {
                return RedirectToAction(nameof(AuthorsController.BecomeAuthor), "Authors");
            }

            var newsArticle = this.news.DetailsWithNoViewCountIncrement(id);

            if (newsArticle == null)
            {
                return NotFound();
            }

            if (newsArticle.UserId != userId && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            var newsArticleData = this.mapper.Map<NewsArticleDeleteDetailsViewModel>(newsArticle);

            newsArticleData.Categories = this.news.Categories();
            newsArticleData.Tags = this.news.Tags();

            return View(newsArticleData);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var authorId = this.authors.AuthorId(this.User.Id());

            if (authorId == 0 && !this.User.IsAdmin())
            {
                return RedirectToAction(nameof(AuthorsController.BecomeAuthor), "Authors");
            }

            if (!this.news.IsByAuthor(id, authorId) && !this.User.IsAdmin())
            {
                return BadRequest();
            }

            this.news.Delete(id);

            TempData[GlobalMessageKey] = "Your news article was deleted successfully!";

            return RedirectToAction(nameof(All));
        }
    }
}
