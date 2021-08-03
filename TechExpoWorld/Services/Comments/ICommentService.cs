namespace TechExpoWorld.Services.Comments
{
    using System.Collections.Generic;

    public interface ICommentService
    {
        IEnumerable<CommentServiceModel> CommentsOnNewsArticle(int newsArticleId);

        int Create(int newsArticleId, string content, string userId);

        int TotalCommentsOnNewsArticle(int newsArticleId);
    }
}
