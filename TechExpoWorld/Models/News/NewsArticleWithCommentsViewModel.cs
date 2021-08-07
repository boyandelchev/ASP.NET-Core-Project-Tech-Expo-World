namespace TechExpoWorld.Models.News
{
    using System.Collections.Generic;
    using TechExpoWorld.Models.Comments;
    using TechExpoWorld.Services.Comments;
    using TechExpoWorld.Services.News;

    public class NewsArticleWithCommentsViewModel
    {
        public int NewsArticleId { get; init; }

        public NewsArticleDetailsServiceModel NewsArticle { get; init; }

        public CommentFormModel Comment { get; init; }

        public IEnumerable<CommentServiceModel> Comments { get; init; }

        public int TotalComments { get; init; }
    }
}
