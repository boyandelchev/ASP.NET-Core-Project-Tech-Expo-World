namespace TechExpoWorld.Services.News
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Microsoft.EntityFrameworkCore;

    using TechExpoWorld.Data;
    using TechExpoWorld.Data.Models;
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

        public async Task<NewsArticlesQueryServiceModel> AllAsync(
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
                    .Where(na => na.Category.Name == category);
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

            var totalNewsArticles = await newsQuery.CountAsync();

            var news = await GetNewsArticlesAsync(newsQuery
                .Skip((currentPage - 1) * newsArticlesPerPage)
                .Take(newsArticlesPerPage));

            return new NewsArticlesQueryServiceModel
            {
                TotalNewsArticles = totalNewsArticles,
                News = news
            };
        }

        public async Task<IEnumerable<NewsArticleServiceModel>> NewsArticlesByAuthorAsync(string authorId)
            => await GetNewsArticlesAsync(this.data
                .NewsArticles
                .Where(na => na.AuthorId == authorId)
                .OrderByDescending(na => na.Id));

        public async Task<IList<LatestNewsArticleServiceModel>> LatestNewsArticlesAsync()
            => await this.data
                .NewsArticles
                .OrderByDescending(c => c.Id)
                .Take(3)
                .ProjectTo<LatestNewsArticleServiceModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<NewsArticleDetailsServiceModel> DetailsAsync(int newsArticleId)
        {
            var isIncremented = await ViewCountIncrementAsync(newsArticleId);

            if (!isIncremented)
            {
                return null;
            }

            var newsArticle = await DetailsWithNoViewCountIncrementAsync(newsArticleId);

            return newsArticle;
        }

        public async Task<NewsArticleDetailsServiceModel> DetailsWithNoViewCountIncrementAsync(int newsArticleId)
            => await this.data
                .NewsArticles
                .Where(na => na.Id == newsArticleId)
                .ProjectTo<NewsArticleDetailsServiceModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

        public async Task<int> CreateAsync(
            string title,
            string content,
            string imageUrl,
            int categoryId,
            IEnumerable<int> tagIds,
            string authorId)
        {
            var newsArticle = new NewsArticle
            {
                Title = title,
                Content = content,
                ImageUrl = imageUrl,
                CategoryId = categoryId,
                NewsArticleTags = CreateNewsArticleTags(tagIds),
                AuthorId = authorId
            };

            await this.data.NewsArticles.AddAsync(newsArticle);
            await this.data.SaveChangesAsync();

            return newsArticle.Id;
        }

        public async Task<bool> EditAsync(
            int newsArticleId,
            string title,
            string content,
            string imageUrl,
            int categoryId,
            IEnumerable<int> tagIds)
        {
            var newsArticle = await this.data
                .NewsArticles
                .Include(na => na.NewsArticleTags)
                .FirstOrDefaultAsync(na => na.Id == newsArticleId);

            if (newsArticle == null)
            {
                return false;
            }

            newsArticle.Title = title;
            newsArticle.Content = content;
            newsArticle.ImageUrl = imageUrl;
            newsArticle.LastModifiedOn = DateTime.UtcNow;
            newsArticle.CategoryId = categoryId;
            newsArticle.NewsArticleTags = CreateNewsArticleTags(tagIds);

            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int newsArticleId)
        {
            var newsArticle = await this.data
                .NewsArticles
                .Include(na => na.NewsArticleTags)
                .Include(na => na.Comments)
                .FirstOrDefaultAsync(na => na.Id == newsArticleId);

            if (newsArticle == null)
            {
                return false;
            }

            newsArticle.NewsArticleTags = null;
            newsArticle.Comments = null;

            this.data.NewsArticles.Remove(newsArticle);
            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<bool> IsByAuthorAsync(int newsArticleId, string authorId)
            => await this.data
                .NewsArticles
                .AnyAsync(na => na.Id == newsArticleId && na.AuthorId == authorId);

        public async Task<IEnumerable<CategoryServiceModel>> CategoriesAsync()
            => await this.data
                .Categories
                .ProjectTo<CategoryServiceModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<IEnumerable<string>> CategoryNamesAsync()
            => await this.data
                .Categories
                .Select(c => c.Name)
                .ToListAsync();

        public async Task<bool> CategoryExistsAsync(int categoryId)
            => await this.data
                .Categories
                .AnyAsync(c => c.Id == categoryId);

        public async Task<IEnumerable<TagServiceModel>> TagsAsync()
            => await this.data
                .Tags
                .ProjectTo<TagServiceModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<IEnumerable<string>> TagNamesAsync()
            => await this.data
                .Tags
                .Select(t => t.Name)
                .ToListAsync();

        public bool TagsExist(IEnumerable<int> tagIds)
            => tagIds.All(tagId => TagExistsAsync(tagId).GetAwaiter().GetResult());

        private static IEnumerable<NewsArticleTag> CreateNewsArticleTags(IEnumerable<int> tagIds)
        {
            var newsArticleTags = new List<NewsArticleTag>();

            foreach (var tagId in tagIds)
            {
                newsArticleTags.Add(new NewsArticleTag { TagId = tagId });
            }

            return newsArticleTags;
        }

        private async Task<bool> TagExistsAsync(int tagId)
            => await this.data
                .Tags
                .AnyAsync(t => t.Id == tagId);

        private async Task<IEnumerable<NewsArticleServiceModel>> GetNewsArticlesAsync(IQueryable<NewsArticle> newsQuery)
            => await newsQuery
                .ProjectTo<NewsArticleServiceModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        private async Task<bool> ViewCountIncrementAsync(int newsArticleId)
        {
            var newsArticle = await this.data.NewsArticles.FindAsync(newsArticleId);

            if (newsArticle == null)
            {
                return false;
            }

            newsArticle.ViewCount += 1;

            await this.data.SaveChangesAsync();

            return true;
        }
    }
}
