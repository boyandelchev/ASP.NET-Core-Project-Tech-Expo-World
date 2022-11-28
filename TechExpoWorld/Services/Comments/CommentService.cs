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
        private readonly TechExpoDbContext data;
        private readonly IMapper mapper;

        public CommentService(TechExpoDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<CommentServiceModel>> CommentsOnNewsArticle(int newsArticleId)
            => await this.data
                .NewsArticles
                .Where(na => na.Id == newsArticleId)
                .SelectMany(na => na.Comments)
                .ProjectTo<CommentServiceModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<int> Create(int newsArticleId, string content, string userId)
        {
            var comment = new Comment
            {
                Content = content,
                NewsArticleId = newsArticleId,
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
    }
}
