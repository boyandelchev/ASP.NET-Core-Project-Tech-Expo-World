namespace TechExpoWorld.Services.Comments
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TechExpoWorld.Services.Comments.Models;

    public interface ICommentsService
    {
        Task<IEnumerable<CommentServiceModel>> CommentsOnNewsArticleAsync(int newsArticleId);

        Task<int> CreateAsync(
            int newsArticleId,
            string content,
            int? parentCommentId,
            string userId);

        Task<int> TotalCommentsOnNewsArticleAsync(int newsArticleId);
    }
}
