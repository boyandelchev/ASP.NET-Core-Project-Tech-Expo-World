namespace TechExpoWorld.Services.Data.News
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using TechExpoWorld.Data.Common.Repositories;
    using TechExpoWorld.Data.Models;
    using TechExpoWorld.Services.Mapping;

    public class NewsService : INewsService
    {
        private readonly IDeletableEntityRepository<NewsArticle> newsArticlesRepository;
        private readonly IDeletableEntityRepository<Category> categoriesRepository;
        private readonly IDeletableEntityRepository<Tag> tagsRepository;

        public NewsService(
            IDeletableEntityRepository<NewsArticle> newsArticlesRepository,
            IDeletableEntityRepository<Category> categoriesRepository,
            IDeletableEntityRepository<Tag> tagsRepository)
        {
            this.newsArticlesRepository = newsArticlesRepository;
            this.categoriesRepository = categoriesRepository;
            this.tagsRepository = tagsRepository;
        }

        public async Task<(IEnumerable<T> NewsArticles, int TotalNewsArticles)> AllAsync<T>(
            string category,
            string tag,
            string searchTerm,
            int sorting,
            int currentPage,
            int newsArticlesPerPage)
        {
            var newsQuery = this.newsArticlesRepository.All().AsQueryable();

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

            newsQuery = (NewsSorting)sorting switch
            {
                NewsSorting.Ascending => newsQuery.OrderBy(na => na.Id),
                NewsSorting.Descending or _ => newsQuery.OrderByDescending(na => na.Id),
            };

            var totalNewsArticles = await newsQuery.CountAsync();

            var newsArticles = await GetNewsArticlesAsync<T>(newsQuery
                .Skip((currentPage - 1) * newsArticlesPerPage)
                .Take(newsArticlesPerPage));

            return (newsArticles, totalNewsArticles);
        }

        public async Task<IEnumerable<T>> NewsArticlesByAuthorAsync<T>(string authorId)
            => await GetNewsArticlesAsync<T>(this.newsArticlesRepository
                .All()
                .Where(na => na.AuthorId == authorId)
                .OrderByDescending(na => na.Id));

        public async Task<IList<T>> LatestNewsArticlesAsync<T>()
            => await this.newsArticlesRepository
                .All()
                .OrderByDescending(c => c.Id)
                .Take(3)
                .To<T>()
                .ToListAsync();

        public async Task<T> DetailsAsync<T>(int newsArticleId)
        {
            var isIncremented = await this.ViewCountIncrementAsync(newsArticleId);

            if (!isIncremented)
            {
                return default;
            }

            var newsArticle = await this.DetailsWithNoViewCountIncrementAsync<T>(newsArticleId);

            return newsArticle;
        }

        public async Task<T> DetailsWithNoViewCountIncrementAsync<T>(int newsArticleId)
            => await this.newsArticlesRepository
                .All()
                .Where(na => na.Id == newsArticleId)
                .To<T>()
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
                AuthorId = authorId,
            };

            await this.newsArticlesRepository.AddAsync(newsArticle);
            await this.newsArticlesRepository.SaveChangesAsync();

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
            var newsArticle = await this.newsArticlesRepository
                .All()
                .Include(na => na.NewsArticleTags)
                .FirstOrDefaultAsync(na => na.Id == newsArticleId);

            if (newsArticle == null)
            {
                return false;
            }

            newsArticle.Title = title;
            newsArticle.Content = content;
            newsArticle.ImageUrl = imageUrl;
            newsArticle.CategoryId = categoryId;
            newsArticle.NewsArticleTags = CreateNewsArticleTags(tagIds);

            await this.newsArticlesRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int newsArticleId)
        {
            var newsArticle = await this.newsArticlesRepository
                .All()
                .Include(na => na.NewsArticleTags)
                .Include(na => na.Comments)
                .FirstOrDefaultAsync(na => na.Id == newsArticleId);

            if (newsArticle == null)
            {
                return false;
            }

            newsArticle.NewsArticleTags = null;
            newsArticle.Comments = null;

            this.newsArticlesRepository.Delete(newsArticle);
            await this.newsArticlesRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> IsByAuthorAsync(int newsArticleId, string authorId)
            => await this.newsArticlesRepository
                .All()
                .AnyAsync(na => na.Id == newsArticleId && na.AuthorId == authorId);

        public async Task<IEnumerable<T>> CategoriesAsync<T>()
            => await this.categoriesRepository
                .All()
                .To<T>()
                .ToListAsync();

        public async Task<IEnumerable<string>> CategoryNamesAsync()
            => await this.categoriesRepository
                .All()
                .Select(c => c.Name)
                .ToListAsync();

        public async Task<bool> CategoryExistsAsync(int categoryId)
            => await this.categoriesRepository
                .All()
                .AnyAsync(c => c.Id == categoryId);

        public async Task<IEnumerable<T>> TagsAsync<T>()
            => await this.tagsRepository
                .All()
                .To<T>()
                .ToListAsync();

        public async Task<IEnumerable<string>> TagNamesAsync()
            => await this.tagsRepository
                .All()
                .Select(t => t.Name)
                .ToListAsync();

        public bool TagsExist(IEnumerable<int> tagIds)
            => tagIds.All(tagId => this.TagExistsAsync(tagId).GetAwaiter().GetResult());

        private static async Task<IEnumerable<T>> GetNewsArticlesAsync<T>(IQueryable<NewsArticle> newsQuery)
            => await newsQuery
                .To<T>()
                .ToListAsync();

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
            => await this.tagsRepository
                .All()
                .AnyAsync(t => t.Id == tagId);

        private async Task<bool> ViewCountIncrementAsync(int newsArticleId)
        {
            var newsArticle = await this.newsArticlesRepository
                .All()
                .FirstOrDefaultAsync(na => na.Id == newsArticleId);

            if (newsArticle == null)
            {
                return false;
            }

            newsArticle.ViewCount += 1;

            await this.newsArticlesRepository.SaveChangesAsync();

            return true;
        }
    }
}
