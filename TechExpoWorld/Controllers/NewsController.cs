namespace TechExpoWorld.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TechExpoWorld.Infrastructure;
    using TechExpoWorld.Models.Comments;
    using TechExpoWorld.Models.News;
    using TechExpoWorld.Services.Authors;
    using TechExpoWorld.Services.Comments;
    using TechExpoWorld.Services.News;

    public class NewsController : Controller
    {
        private readonly INewsService news;
        private readonly IAuthorService authors;
        private readonly ICommentService comments;

        public NewsController(INewsService news, IAuthorService authors, ICommentService comments)
        {
            this.news = news;
            this.authors = authors;
            this.comments = comments;
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

        public IActionResult Details(int id)
        {
            this.news.ViewCountIncrement(id);

            var newsArticle = this.news.Details(id);
            var comments = this.comments.CommentsOnNewsArticle(id);
            var totalComments = this.comments.TotalCommentsOnNewsArticle(id);

            return View(new NewsArticleWithCommentsViewModel
            {
                NewsArticle = newsArticle,
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

            this.news.Create(
                newsArticle.Title,
                newsArticle.Content,
                newsArticle.ImageUrl,
                newsArticle.CategoryId,
                newsArticle.TagIds,
                authorId);

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var userId = this.User.Id();

            if (!this.authors.IsAuthor(userId) && !this.User.IsAdmin())
            {
                return RedirectToAction(nameof(AuthorsController.BecomeAuthor), "Authors");
            }

            var newsArticle = this.news.Details(id);

            if (newsArticle == null)
            {
                return NotFound();
            }

            if (newsArticle.UserId != userId && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            return View(new NewsArticleFormModel
            {
                Title = newsArticle.Title,
                Content = newsArticle.Content,
                ImageUrl = newsArticle.ImageUrl,
                CategoryId = newsArticle.CategoryId,
                TagIds = newsArticle.TagIds,
                Categories = this.news.Categories(),
                Tags = this.news.Tags()
            });
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

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult DeleteDetails(int id)
        {
            var userId = this.User.Id();

            if (!this.authors.IsAuthor(userId) && !this.User.IsAdmin())
            {
                return RedirectToAction(nameof(AuthorsController.BecomeAuthor), "Authors");
            }

            var newsArticle = this.news.Details(id);

            if (newsArticle == null)
            {
                return NotFound();
            }

            if (newsArticle.UserId != userId && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            return View(new NewsArticleDeleteDetailsViewModel
            {
                Id = newsArticle.Id,
                Title = newsArticle.Title,
                Content = newsArticle.Content,
                ImageUrl = newsArticle.ImageUrl,
                CategoryId = newsArticle.CategoryId,
                TagIds = newsArticle.TagIds,
                Categories = this.news.Categories(),
                Tags = this.news.Tags()
            });
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

            return RedirectToAction(nameof(All));
        }
    }
}
