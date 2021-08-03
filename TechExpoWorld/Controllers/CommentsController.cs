namespace TechExpoWorld.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TechExpoWorld.Infrastructure;
    using TechExpoWorld.Models.Comments;
    using TechExpoWorld.Services.Comments;

    public class CommentsController : Controller
    {
        private readonly ICommentService comments;

        public CommentsController(ICommentService comments)
            => this.comments = comments;

        [HttpPost]
        [Authorize]
        public IActionResult Add(int id, CommentFormModel comment)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(NewsController.Details), "News", new { id });
            }

            var userId = this.User.Id();

            this.comments.Create(id, comment.Content, userId);

            return RedirectToAction(nameof(NewsController.Details), "News", new { id });
        }
    }
}
