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
        private readonly ICommentService comments;
        private readonly INewsService news;

        public CommentsController(ICommentService comments, INewsService news)
        {
            this.comments = comments;
            this.news = news;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(int id, CommentFormModel comment)
        {
            var newsArticle = await this.news.DetailsWithNoViewCountIncrement(id);

            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(NewsController.Details),
                                        ControllerNews,
                                        new
                                        {
                                            id,
                                            information = newsArticle.GetNewsArticleInformation()
                                        });
            }

            var userId = this.User.Id();

            await this.comments.Create(id, comment.Content, userId);

            TempData[GlobalMessageKey] = CreatedComment;

            return RedirectToAction(nameof(NewsController.Details),
                                    ControllerNews,
                                    new
                                    {
                                        id,
                                        information = newsArticle.GetNewsArticleInformation()
                                    });
        }
    }
}
