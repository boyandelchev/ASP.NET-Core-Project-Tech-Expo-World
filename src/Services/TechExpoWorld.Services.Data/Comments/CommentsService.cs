namespace TechExpoWorld.Services.Data.Comments
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using TechExpoWorld.Data.Common.Repositories;
    using TechExpoWorld.Data.Models;
    using TechExpoWorld.Services.Mapping;

    public class CommentsService : ICommentsService
    {
        private const int CommentMaxDepth = 5;
        private readonly IDeletableEntityRepository<Comment> commentsRepository;

        public CommentsService(IDeletableEntityRepository<Comment> commentsRepository)
            => this.commentsRepository = commentsRepository;

        public async Task<IEnumerable<CommentServiceModel>> CommentsOfNewsArticleAsync(int newsArticleId)
        {
            var comments = await this.commentsRepository
                .All()
                .Where(c => c.NewsArticleId == newsArticleId)
                .To<CommentServiceModel>()
                .ToListAsync();

            var childrenCommentsById = comments
                .ToDictionary(c => c.Id, c => c.ChildrenComments
                    .OrderByDescending(c => c.Id)
                    .AsEnumerable());

            var parentComments = comments
                .Where(c => c.ParentCommentId == null)
                .OrderByDescending(c => c.Id);

            foreach (var c in parentComments)
            {
                c.ChildrenComments = childrenCommentsById[c.Id];
            }

            foreach (var c in parentComments)
            {
                AddChildrenComments(c, childrenCommentsById);
            }

            return parentComments;
        }

        public async Task<int> CreateAsync(
            int newsArticleId,
            string content,
            int? parentCommentId,
            string userId)
        {
            var parentComment = await this.CommentAsync(parentCommentId);

            if (parentCommentId != null && parentComment == null)
            {
                return 0;
            }

            int depth;

            if (parentCommentId == null)
            {
                depth = 0;
            }
            else if (parentComment.Depth == CommentMaxDepth)
            {
                depth = 0;
                parentCommentId = null;
            }
            else
            {
                depth = parentComment.Depth + 1;
            }

            var comment = new Comment
            {
                NewsArticleId = newsArticleId,
                Content = content,
                Depth = depth,
                ParentCommentId = parentCommentId,
                UserId = userId,
            };

            await this.commentsRepository.AddAsync(comment);
            await this.commentsRepository.SaveChangesAsync();

            return comment.Id;
        }

        public async Task<int> TotalCommentsOfNewsArticleAsync(int newsArticleId)
            => await this.commentsRepository
                .All()
                .Where(c => c.NewsArticleId == newsArticleId)
                .CountAsync();

        private static void AddChildrenComments(
            CommentServiceModel comment,
            IDictionary<int, IEnumerable<CommentServiceModel>> childrenCommentsById)
        {
            foreach (var c in comment.ChildrenComments)
            {
                c.ChildrenComments = childrenCommentsById[c.Id];

                AddChildrenComments(c, childrenCommentsById);
            }
        }

        private async Task<Comment> CommentAsync(int? commentId)
            => await this.commentsRepository
                .All()
                .FirstOrDefaultAsync(c => c.Id == commentId);
    }
}
