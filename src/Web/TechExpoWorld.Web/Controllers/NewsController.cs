namespace TechExpoWorld.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using TechExpoWorld.Services.Data.Authors;
    using TechExpoWorld.Services.Data.Comments;
    using TechExpoWorld.Services.Data.News;
    using TechExpoWorld.Web.Infrastructure.Extensions;
    using TechExpoWorld.Web.ViewModels.Comments;
    using TechExpoWorld.Web.ViewModels.News;

    using static TechExpoWorld.Common.GlobalConstants.NewsArticle;
    using static TechExpoWorld.Common.GlobalConstants.TempData;

    public class NewsController : BaseController
    {
        private const string ControllerAuthors = "Authors";
        private readonly INewsService newsService;
        private readonly IAuthorsService authorsService;
        private readonly ICommentsService commentsService;

        public NewsController(
            INewsService newsService,
            IAuthorsService authorsService,
            ICommentsService commentsService)
        {
            this.newsService = newsService;
            this.authorsService = authorsService;
            this.commentsService = commentsService;
        }

        public async Task<IActionResult> All(AllNewsArticlesQueryViewModel query)
        {
            var (newsArticles, totalNewsArticles) = await this.newsService.AllAsync<NewsArticleViewModel>(
                query.Category,
                query.Tag,
                query.SearchTerm,
                (int)query.Sorting,
                query.CurrentPage,
                AllNewsArticlesQueryViewModel.NewsArticlesPerPage);

            query.Categories = await this.newsService.CategoryNamesAsync();
            query.Tags = await this.newsService.TagNamesAsync();
            query.TotalNewsArticles = totalNewsArticles;
            query.NewsArticles = newsArticles;

            return this.View(query);
        }

        public async Task<IActionResult> Details(int id, string information)
        {
            var newsArticle = await this.newsService.DetailsAsync<NewsArticleDetailsViewModel>(id);

            if (newsArticle == null)
            {
                return this.NotFound();
            }

            if (information != newsArticle.GetNewsArticleInformation())
            {
                return this.BadRequest();
            }

            var comments = await this.commentsService.CommentsOfNewsArticleAsync(id);
            var totalComments = await this.commentsService.TotalCommentsOfNewsArticleAsync(id);

            return this.View(new NewsArticleWithCommentsViewModel
            {
                NewsArticle = newsArticle,
                CommentInput = new CommentInputModel(),
                Comments = comments,
                TotalComments = totalComments,
            });
        }

        [Authorize]
        public async Task<IActionResult> NewsArticlesMine()
        {
            if (this.User.IsAdmin())
            {
                return this.BadRequest();
            }

            var authorId = await this.authorsService.AuthorIdAsync(this.User.Id());

            var newsArticlesMine = await this.newsService.NewsArticlesByAuthorAsync<NewsArticleViewModel>(authorId);

            return this.View(newsArticlesMine);
        }

        [Authorize]
        public async Task<IActionResult> Add()
        {
            if (this.User.IsAdmin())
            {
                return this.BadRequest();
            }

            if (!await this.authorsService.IsAuthorAsync(this.User.Id()))
            {
                return this.RedirectToAction(nameof(AuthorsController.BecomeAuthor), ControllerAuthors);
            }

            return this.View(new NewsArticleInputModel
            {
                Categories = await this.newsService.CategoriesAsync<CategoryViewModel>(),
                Tags = await this.newsService.TagsAsync<TagViewModel>(),
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(NewsArticleInputModel input)
        {
            if (this.User.IsAdmin())
            {
                return this.BadRequest();
            }

            var authorId = await this.authorsService.AuthorIdAsync(this.User.Id());

            if (authorId == null)
            {
                return this.RedirectToAction(nameof(AuthorsController.BecomeAuthor), ControllerAuthors);
            }

            if (!await this.newsService.CategoryExistsAsync(input.CategoryId))
            {
                this.ModelState.AddModelError(nameof(input.CategoryId), ErrorCategory);
            }

            if (!this.newsService.TagsExist(input.TagIds))
            {
                this.ModelState.AddModelError(nameof(input.TagIds), ErrorTag);
            }

            if (!this.ModelState.IsValid)
            {
                input.Categories = await this.newsService.CategoriesAsync<CategoryViewModel>();
                input.Tags = await this.newsService.TagsAsync<TagViewModel>();

                return this.View(input);
            }

            var newsArticleId = await this.newsService.CreateAsync(
                input.Title,
                input.Content,
                input.ImageUrl,
                input.CategoryId,
                input.TagIds,
                authorId);

            this.TempData[GlobalMessageKey] = CreatedNewsArticle;

            return this.RedirectToAction(
                nameof(this.Details),
                new { id = newsArticleId, information = input.GetNewsArticleInformation() });
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var authorId = await this.authorsService.AuthorIdAsync(this.User.Id());

            if (authorId == null && !this.User.IsAdmin())
            {
                return this.RedirectToAction(nameof(AuthorsController.BecomeAuthor), ControllerAuthors);
            }

            var input = await this.newsService.DetailsWithNoViewCountIncrementAsync<NewsArticleInputModel>(id);

            if (input == null)
            {
                return this.NotFound();
            }

            if (!await this.newsService.IsByAuthorAsync(id, authorId) && !this.User.IsAdmin())
            {
                return this.Unauthorized();
            }

            input.Categories = await this.newsService.CategoriesAsync<CategoryViewModel>();
            input.Tags = await this.newsService.TagsAsync<TagViewModel>();

            return this.View(input);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(int id, NewsArticleInputModel input)
        {
            var authorId = await this.authorsService.AuthorIdAsync(this.User.Id());

            if (authorId == null && !this.User.IsAdmin())
            {
                return this.RedirectToAction(nameof(AuthorsController.BecomeAuthor), ControllerAuthors);
            }

            if (!await this.newsService.IsByAuthorAsync(id, authorId) && !this.User.IsAdmin())
            {
                return this.Unauthorized();
            }

            if (!await this.newsService.CategoryExistsAsync(input.CategoryId))
            {
                this.ModelState.AddModelError(nameof(input.CategoryId), ErrorCategory);
            }

            if (!this.newsService.TagsExist(input.TagIds))
            {
                this.ModelState.AddModelError(nameof(input.TagIds), ErrorTag);
            }

            if (!this.ModelState.IsValid)
            {
                input.Categories = await this.newsService.CategoriesAsync<CategoryViewModel>();
                input.Tags = await this.newsService.TagsAsync<TagViewModel>();

                return this.View(input);
            }

            await this.newsService.EditAsync(
                id,
                input.Title,
                input.Content,
                input.ImageUrl,
                input.CategoryId,
                input.TagIds);

            this.TempData[GlobalMessageKey] = EditedNewsArticle;

            return this.RedirectToAction(
                nameof(this.Details),
                new { id, information = input.GetNewsArticleInformation() });
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id) => await this.Edit(id);

        [ActionName(nameof(Delete))]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var authorId = await this.authorsService.AuthorIdAsync(this.User.Id());

            if (authorId == null && !this.User.IsAdmin())
            {
                return this.RedirectToAction(nameof(AuthorsController.BecomeAuthor), ControllerAuthors);
            }

            if (!await this.newsService.IsByAuthorAsync(id, authorId) && !this.User.IsAdmin())
            {
                return this.Unauthorized();
            }

            await this.newsService.DeleteAsync(id);

            this.TempData[GlobalMessageKey] = DeletedNewsArticle;

            return this.RedirectToAction(nameof(this.All));
        }
    }
}
