namespace TechExpoWorld.Models.News
{
    using System.Collections.Generic;

    using TechExpoWorld.Models.Comments;
    using TechExpoWorld.Services.Comments.Models;
    using TechExpoWorld.Services.News.Models;

    public class NewsArticleWithCommentsViewModel
    {
        public NewsArticleDetailsServiceModel NewsArticle { get; init; }

        public CommentFormModel CommentForm { get; init; }

        public IEnumerable<CommentServiceModel> Comments { get; init; }

        public int TotalComments { get; init; }
    }
}
