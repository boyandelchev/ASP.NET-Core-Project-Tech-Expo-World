namespace TechExpoWorld.Web.ViewModels.News
{
    using System.Collections.Generic;

    using TechExpoWorld.Services.Data.Comments;
    using TechExpoWorld.Web.ViewModels.Comments;

    public class NewsArticleWithCommentsViewModel
    {
        public NewsArticleDetailsViewModel NewsArticle { get; init; }

        public CommentInputModel CommentInput { get; init; }

        public IEnumerable<CommentServiceModel> Comments { get; init; }

        public int TotalComments { get; init; }
    }
}
