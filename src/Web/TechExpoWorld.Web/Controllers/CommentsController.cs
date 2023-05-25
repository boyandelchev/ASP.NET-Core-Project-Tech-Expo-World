namespace TechExpoWorld.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using TechExpoWorld.Services.Data.Comments;
    using TechExpoWorld.Services.Data.News;
    using TechExpoWorld.Web.Infrastructure.Extensions;
    using TechExpoWorld.Web.ViewModels.Comments;
    using TechExpoWorld.Web.ViewModels.News;

    using static TechExpoWorld.Common.GlobalConstants.TempData;

    public class CommentsController : BaseController
    {
        private const string ControllerNews = "News";
        private readonly ICommentsService commentsService;
        private readonly INewsService newsService;

        public CommentsController(
            ICommentsService commentsService,
            INewsService newsService)
        {
            this.commentsService = commentsService;
            this.newsService = newsService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(int id, CommentInputModel input)
        {
            var newsArticle = await this.newsService
                .DetailsWithNoViewCountIncrementAsync<NewsArticleTitleViewModel>(id);

            var redirectToAction = this.RedirectToAction(
                nameof(NewsController.Details),
                ControllerNews,
                new { id, information = newsArticle.GetNewsArticleInformation() });

            if (!this.ModelState.IsValid)
            {
                return redirectToAction;
            }

            var userId = this.User.Id();

            var commentId = await this.commentsService.CreateAsync(
                id,
                input.Content,
                input.ParentCommentId,
                userId);

            if (commentId != 0)
            {
                this.TempData[GlobalMessageKey] = CreatedComment;
            }

            return redirectToAction;
        }
    }
}
