namespace TechExpoWorld.Web.ViewModels.News
{
    using System.Collections.Generic;

    using TechExpoWorld.Web.ViewModels.Comments;

    public class NewsArticleWithCommentsViewModel
    {
        public NewsArticleDetailsViewModel NewsArticle { get; init; }

        public CommentInputModel CommentForm { get; init; }

        public IEnumerable<CommentViewModel> Comments { get; init; }

        public int TotalComments { get; init; }
    }
}
