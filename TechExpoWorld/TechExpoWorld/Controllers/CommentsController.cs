namespace TechExpoWorld.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using TechExpoWorld.Infrastructure.Extensions;
    using TechExpoWorld.Models.Comments;
    using TechExpoWorld.Services.Comments;
    using TechExpoWorld.Services.News;

    using static GlobalConstants.TempData;

    public class CommentsController : Controller
    {
        private const string ControllerNews = "News";
        private readonly ICommentsService comments;
        private readonly INewsService news;

        public CommentsController(ICommentsService comments, INewsService news)
        {
            this.comments = comments;
            this.news = news;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(int id, CommentFormModel commentForm)
        {
            var newsArticle = await this.news.DetailsWithNoViewCountIncrementAsync(id);

            var redirectToAction = RedirectToAction(
                nameof(NewsController.Details),
                ControllerNews,
                new { id, information = newsArticle.GetNewsArticleInformation() });

            if (!ModelState.IsValid)
            {
                return redirectToAction;
            }

            var userId = this.User.Id();

            var commentId = await this.comments.CreateAsync(
                id,
                commentForm.Content,
                commentForm.ParentCommentId,
                userId);

            if (commentId != 0)
            {
                TempData[GlobalMessageKey] = CreatedComment;
            }

            return redirectToAction;
        }
    }
}
