namespace TechExpoWorld.Services.News
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using TechExpoWorld.Data;
    using TechExpoWorld.Data.Models;
    using TechExpoWorld.Models.News;
    using TechExpoWorld.Services.News.Models;

    public class NewsService : INewsService
    {
        private readonly TechExpoDbContext data;
        private readonly IMapper mapper;

        public NewsService(TechExpoDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

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
                .Where(na => na.Author.UserId == userId)
                .OrderByDescending(na => na.Id));

        public IEnumerable<LatestNewsArticleServiceModel> LatestNewsArticles()
            => this.data
                .NewsArticles
                .OrderByDescending(c => c.Id)
                .ProjectTo<LatestNewsArticleServiceModel>(this.mapper.ConfigurationProvider)
                .Take(3)
                .ToList();

        public NewsArticleDetailsServiceModel Details(int newsArticleId)
        {
            var isIncremented = ViewCountIncrement(newsArticleId);

            if (!isIncremented)
            {
                return null;
            }

            var newsArticle = DetailsWithNoViewCountIncrement(newsArticleId);

            return newsArticle;
        }

        public NewsArticleDetailsServiceModel DetailsWithNoViewCountIncrement(int newsArticleId)
            => this.data
                .NewsArticles
                .Where(na => na.Id == newsArticleId)
                .ProjectTo<NewsArticleDetailsServiceModel>(this.mapper.ConfigurationProvider)
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

        public bool Delete(int newsArticleId)
        {
            var newsArticle = this.data
                .NewsArticles
                .Include(na => na.NewsArticleTags)
                .FirstOrDefault(na => na.Id == newsArticleId);

            if (newsArticle == null)
            {
                return false;
            }

            newsArticle.NewsArticleTags = null;

            this.data.NewsArticles.Remove(newsArticle);
            this.data.SaveChanges();

            return true;
        }

        public bool IsByAuthor(int newsArticleId, int authorId)
            => this.data
                .NewsArticles
                .Any(na => na.Id == newsArticleId && na.AuthorId == authorId);

        public IEnumerable<CategoryServiceModel> Categories()
            => this.data
                .NewsCategories
                .ProjectTo<CategoryServiceModel>(this.mapper.ConfigurationProvider)
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
                .ProjectTo<TagServiceModel>(this.mapper.ConfigurationProvider)
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

        private bool TagExists(int tagId)
            => this.data
                .Tags
                .Any(t => t.Id == tagId);

        private IEnumerable<NewsArticleServiceModel> GetNewsArticles(IQueryable<NewsArticle> newsQuery)
            => newsQuery
                .ProjectTo<NewsArticleServiceModel>(this.mapper.ConfigurationProvider)
                .ToList();

        private bool ViewCountIncrement(int newsArticleId)
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
    }
}
