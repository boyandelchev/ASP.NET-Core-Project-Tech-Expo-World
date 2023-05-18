namespace TechExpoWorld.Services.Comments
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TechExpoWorld.Services.Comments.Models;

    public interface ICommentService
    {
        Task<IEnumerable<CommentServiceModel>> CommentsOnNewsArticle(int newsArticleId);

        Task<int> Create(
            int newsArticleId,
            string content,
            int? parentCommentId,
            string userId);

        Task<int> TotalCommentsOnNewsArticle(int newsArticleId);
    }
}
