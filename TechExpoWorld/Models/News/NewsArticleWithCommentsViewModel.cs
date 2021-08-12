namespace TechExpoWorld.Models.News
{
    using System.Collections.Generic;
    using TechExpoWorld.Models.Comments;
    using TechExpoWorld.Services.Comments.Models;

    public class NewsArticleWithCommentsViewModel
    {
        public int NewsArticleId { get; init; }

        public NewsArticleDetailsViewModel NewsArticle { get; init; }

        public CommentFormModel Comment { get; init; }

        public IEnumerable<CommentServiceModel> Comments { get; init; }

        public int TotalComments { get; init; }
    }
}
