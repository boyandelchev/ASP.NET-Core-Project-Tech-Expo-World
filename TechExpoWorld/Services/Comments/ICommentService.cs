namespace TechExpoWorld.Services.Comments
{
    using System.Collections.Generic;
    using TechExpoWorld.Services.Comments.Models;

    public interface ICommentService
    {
        IEnumerable<CommentServiceModel> CommentsOnNewsArticle(int newsArticleId);

        int Create(int newsArticleId, string content, string userId);

        int TotalCommentsOnNewsArticle(int newsArticleId);
    }
}
