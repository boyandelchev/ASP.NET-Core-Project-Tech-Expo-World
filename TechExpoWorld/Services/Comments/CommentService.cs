namespace TechExpoWorld.Services.Comments
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Microsoft.EntityFrameworkCore;

    using TechExpoWorld.Data;
    using TechExpoWorld.Data.Models;
    using TechExpoWorld.Services.Comments.Models;

    public class CommentService : ICommentService
    {
        private const int CommentMaxDepth = 5;
        private readonly TechExpoDbContext data;
        private readonly IMapper mapper;

        public CommentService(TechExpoDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<CommentServiceModel>> CommentsOnNewsArticle(int newsArticleId)
        {
            var commentsById = await this.data
                .Comments
                .Where(c => c.NewsArticleId == newsArticleId)
                .OrderByDescending(c => c.Id)
                .ProjectTo<CommentServiceModel>(this.mapper.ConfigurationProvider)
                .ToDictionaryAsync(c => c.Id, c => c);

            var childrenCommentsById = commentsById
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ChildrenComments
                    .OrderByDescending(c => c.Id)
                    .AsEnumerable());

            var comments = commentsById.Values.Where(c => c.ParentCommentId == null);

            foreach (var c in comments)
            {
                c.ChildrenComments = childrenCommentsById[c.Id];
            }

            foreach (var c in comments)
            {
                AddChildrenComments(c, childrenCommentsById);
            }

            return comments;
        }

        public async Task<int> Create(
            int newsArticleId,
            string content,
            int? parentCommentId,
            string userId)
        {
            var parentComment = await this.Comment(parentCommentId);

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
                UserId = userId
            };

            await this.data.Comments.AddAsync(comment);
            await this.data.SaveChangesAsync();

            return comment.Id;
        }

        public async Task<int> TotalCommentsOnNewsArticle(int newsArticleId)
            => await this.data
                .Comments
                .Where(c => c.NewsArticleId == newsArticleId)
                .CountAsync();

        private async Task<Comment> Comment(int? commentId)
            => await this.data
                .Comments
                .FindAsync(commentId);

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
    }
}
