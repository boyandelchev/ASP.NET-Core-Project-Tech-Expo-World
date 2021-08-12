namespace TechExpoWorld.Services.Comments
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using TechExpoWorld.Data;
    using TechExpoWorld.Data.Models;
    using TechExpoWorld.Services.Comments.Models;

    public class CommentService : ICommentService
    {
        private readonly TechExpoDbContext data;
        private readonly IMapper mapper;

        public CommentService(TechExpoDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

        public IEnumerable<CommentServiceModel> CommentsOnNewsArticle(int newsArticleId)
            => this.data
                .NewsArticles
                .Where(na => na.Id == newsArticleId)
                .SelectMany(na => na.Comments)
                .ProjectTo<CommentServiceModel>(this.mapper.ConfigurationProvider)
                .ToList();

        public int Create(int newsArticleId, string content, string userId)
        {
            var comment = new Comment
            {
                Content = content,
                NewsArticleId = newsArticleId,
                UserId = userId
            };

            this.data.Comments.Add(comment);
            this.data.SaveChanges();

            return comment.Id;
        }

        public int TotalCommentsOnNewsArticle(int newsArticleId)
            => this.data
                .Comments
                .Where(c => c.NewsArticleId == newsArticleId)
                .Count();
    }
}
