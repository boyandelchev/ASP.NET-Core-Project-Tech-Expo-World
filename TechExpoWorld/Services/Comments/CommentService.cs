namespace TechExpoWorld.Services.Comments
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using TechExpoWorld.Data;
    using TechExpoWorld.Data.Models;

    public class CommentService : ICommentService
    {
        private const string dateFormat = "dd.MM.yyyy HH:mm";
        private readonly TechExpoDbContext data;

        public CommentService(TechExpoDbContext data)
            => this.data = data;

        public IEnumerable<CommentServiceModel> CommentsOnNewsArticle(int newsArticleId)
            => this.data
                .NewsArticles
                .Where(na => na.Id == newsArticleId)
                .SelectMany(na => na.Comments)
                .Select(c => new CommentServiceModel
                {
                    Content = c.Content,
                    CreatedOn = c.CreatedOn.ToString(dateFormat, CultureInfo.InvariantCulture),
                    UserName = c.User.UserName
                })
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
