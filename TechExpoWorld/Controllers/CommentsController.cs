namespace TechExpoWorld.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TechExpoWorld.Infrastructure.Extensions;
    using TechExpoWorld.Models.Comments;
    using TechExpoWorld.Services.Comments;
    using TechExpoWorld.Services.News;

    using static WebConstants;

    public class CommentsController : Controller
    {
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
                                        "News",
                                        new
                                        {
                                            id,
                                            information = newsArticle.GetNewsArticleInformation()
                                        });
            }

            var userId = this.User.Id();

            await this.comments.Create(id, comment.Content, userId);

            TempData[GlobalMessageKey] = "Your comment was added successfully!";

            return RedirectToAction(nameof(NewsController.Details),
                                    "News",
                                    new
                                    {
                                        id,
                                        information = newsArticle.GetNewsArticleInformation()
                                    });
        }
    }
}
