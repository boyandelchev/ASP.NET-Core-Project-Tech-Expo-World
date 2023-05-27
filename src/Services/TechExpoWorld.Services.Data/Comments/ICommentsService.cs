namespace TechExpoWorld.Services.Data.Comments
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICommentsService
    {
        Task<IEnumerable<CommentServiceModel>> CommentsOfNewsArticleAsync(int newsArticleId);

        Task<int> CreateAsync(
            int newsArticleId,
            string content,
            int? parentCommentId,
            string userId);

        Task<int> TotalCommentsOfNewsArticleAsync(int newsArticleId);
    }
}
