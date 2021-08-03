namespace TechExpoWorld.Services.News
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using TechExpoWorld.Data;
    using TechExpoWorld.Data.Models;
    using TechExpoWorld.Models.News;

    public class NewsService : INewsService
    {
        private const string dateFormat = "dd.MM.yyyy HH:mm";
        private readonly TechExpoDbContext data;

        public NewsService(TechExpoDbContext data)
            => this.data = data;

        public NewsArticlesQueryServiceModel All(
            string category,
            string tag,
            string searchTerm,
            NewsSorting sorting,
            int currentPage,
            int newsArticlesPerPage)
        {
            var newsQuery = this.data.NewsArticles.AsQueryable();

            if (!string.IsNullOrWhiteSpace(category))
            {
                newsQuery = newsQuery
                    .Where(na => na.NewsCategory.Name == category);
            }

            if (!string.IsNullOrWhiteSpace(tag))
            {
                newsQuery = newsQuery
                    .Where(na => na.NewsArticleTags
                        .Select(nat => nat.Tag.Name)
                        .Contains(tag));
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                newsQuery = newsQuery.Where(na =>
                    na.Author.Name.ToLower().Contains(searchTerm.ToLower()) ||
                    na.Title.ToLower().Contains(searchTerm.ToLower()) ||
                    na.Content.ToLower().Contains(searchTerm.ToLower()));
            }

            newsQuery = sorting switch
            {
                NewsSorting.Ascending => newsQuery.OrderBy(na => na.Id),
                NewsSorting.Descending or _ => newsQuery.OrderByDescending(na => na.Id)
            };

            var totalNewsArticles = newsQuery.Count();

            var news = GetNewsArticles(newsQuery
                .Skip((currentPage - 1) * newsArticlesPerPage)
                .Take(newsArticlesPerPage));

            return new NewsArticlesQueryServiceModel
            {
                TotalNewsArticles = totalNewsArticles,
                News = news
            };
        }

        public IEnumerable<NewsArticleServiceModel> NewsArticlesByUser(string userId)
            => GetNewsArticles(this.data
                .NewsArticles
                .Where(na => na.Author.UserId == userId));

        public List<NewsArticleIndexServiceModel> LatestNewsArticles()
            => this.data
                .NewsArticles
                .OrderByDescending(c => c.Id)
                .Select(na => new NewsArticleIndexServiceModel
                {
                    Id = na.Id,
                    Title = na.Title,
                    ImageUrl = na.ImageUrl
                })
                .Take(3)
                .ToList();

        public NewsArticleDetailsServiceModel Details(int newsArticleId)
            => this.data
                .NewsArticles
                .Where(na => na.Id == newsArticleId)
                .Select(na => new NewsArticleDetailsServiceModel
                {
                    Id = na.Id,
                    Title = na.Title,
                    Content = na.Content,
                    ImageUrl = na.ImageUrl,
                    CreatedOn = na.CreatedOn.ToString(dateFormat, CultureInfo.InvariantCulture),
                    LastModifiedOn = na.LastModifiedOn.Value.ToString(dateFormat, CultureInfo.InvariantCulture),
                    ViewCount = na.ViewCount,
                    CategoryId = na.NewsCategoryId,
                    CategoryName = na.NewsCategory.Name,
                    AuthorId = na.AuthorId,
                    AuthorName = na.Author.Name,
                    TagNames = na.NewsArticleTags.Select(nat => nat.Tag.Name),
                    TagIds = na.NewsArticleTags.Select(nat => nat.TagId),
                    UserId = na.Author.UserId
                })
                .FirstOrDefault();

        public int Create(
            string title,
            string content,
            string imageUrl,
            int categoryId,
            IEnumerable<int> tagIds,
            int authorId)
        {
            var newsArticle = new NewsArticle
            {
                Title = title,
                Content = content,
                ImageUrl = imageUrl,
                NewsCategoryId = categoryId,
                AuthorId = authorId
            };

            newsArticle.NewsArticleTags = CreateNewsArticleTags(tagIds);

            this.data.NewsArticles.Add(newsArticle);
            this.data.SaveChanges();

            return newsArticle.Id;
        }

        public bool Edit(
            int newsArticleId,
            string title,
            string content,
            string imageUrl,
            int categoryId,
            IEnumerable<int> tagIds)
        {
            var newsArticle = this.data
                .NewsArticles
                .Include(na => na.NewsArticleTags)
                .FirstOrDefault(na => na.Id == newsArticleId);

            if (newsArticle == null)
            {
                return false;
            }

            newsArticle.Title = title;
            newsArticle.Content = content;
            newsArticle.ImageUrl = imageUrl;
            newsArticle.LastModifiedOn = DateTime.UtcNow;
            newsArticle.NewsCategoryId = categoryId;
            newsArticle.NewsArticleTags = CreateNewsArticleTags(tagIds);

            this.data.SaveChanges();

            return true;
        }

        public bool IsByAuthor(int newsArticleId, int authorId)
            => this.data
                .NewsArticles
                .Any(na => na.Id == newsArticleId && na.AuthorId == authorId);

        public bool ViewCountIncrement(int newsArticleId)
        {
            var newsArticle = this.data.NewsArticles.Find(newsArticleId);

            if (newsArticle == null)
            {
                return false;
            }

            newsArticle.ViewCount += 1;

            this.data.SaveChanges();

            return true;
        }

        public IEnumerable<CategoryServiceModel> Categories()
            => this.data
                .NewsCategories
                .Select(c => new CategoryServiceModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .OrderBy(c => c.Name)
                .ToList();

        public IEnumerable<string> CategoryNames()
            => this.data
                .NewsCategories
                .Select(nc => nc.Name)
                .OrderBy(name => name)
                .ToList();

        public bool CategoryExists(int categoryId)
            => this.data
                .NewsCategories
                .Any(nc => nc.Id == categoryId);

        public IEnumerable<TagServiceModel> Tags()
            => this.data
                .Tags
                .Select(t => new TagServiceModel
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .OrderBy(t => t.Name)
                .ToList();

        public IEnumerable<string> TagNames()
            => this.data
                .Tags
                .Select(t => t.Name.ToLower())
                .OrderBy(name => name)
                .ToList();

        public bool TagsExist(IEnumerable<int> tagIds)
            => tagIds.All(tagId => TagExists(tagId));

        private bool TagExists(int tagId)
            => this.data
                .Tags
                .Any(t => t.Id == tagId);

        private static IEnumerable<NewsArticleServiceModel> GetNewsArticles(IQueryable<NewsArticle> newsQuery)
            => newsQuery
                .Select(na => new NewsArticleServiceModel
                {
                    Id = na.Id,
                    Title = na.Title,
                    Content = na.Content.Substring(0, 200) + "...",
                    ImageUrl = na.ImageUrl,
                    CreatedOn = na.CreatedOn.ToString(dateFormat, CultureInfo.InvariantCulture),
                    CategoryName = na.NewsCategory.Name,
                    AuthorName = na.Author.Name
                })
                .ToList();

        private static IEnumerable<NewsArticleTag> CreateNewsArticleTags(IEnumerable<int> tagIds)
        {
            var newsArticleTags = new List<NewsArticleTag>();

            if (tagIds.Any())
            {
                foreach (var tagId in tagIds)
                {
                    newsArticleTags.Add(new NewsArticleTag { TagId = tagId });
                }
            }

            return newsArticleTags;
        }
    }
}
