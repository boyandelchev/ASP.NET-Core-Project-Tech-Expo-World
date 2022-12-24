﻿namespace TechExpoWorld.Services.News
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

        public async Task<NewsArticlesQueryServiceModel> All(
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

            var totalNewsArticles = await newsQuery.CountAsync();

            var news = await GetNewsArticles(newsQuery
                .Skip((currentPage - 1) * newsArticlesPerPage)
                .Take(newsArticlesPerPage));

            return new NewsArticlesQueryServiceModel
            {
                TotalNewsArticles = totalNewsArticles,
                News = news
            };
        }

        public async Task<IEnumerable<NewsArticleServiceModel>> NewsArticlesByUser(string userId)
            => await GetNewsArticles(this.data
                .NewsArticles
                .Where(na => na.Author.UserId == userId)
                .OrderByDescending(na => na.Id));

        public async Task<IList<LatestNewsArticleServiceModel>> LatestNewsArticles()
            => await this.data
                .NewsArticles
                .OrderByDescending(c => c.Id)
                .ProjectTo<LatestNewsArticleServiceModel>(this.mapper.ConfigurationProvider)
                .Take(3)
                .ToListAsync();

        public async Task<NewsArticleDetailsServiceModel> Details(int newsArticleId)
        {
            var isIncremented = await ViewCountIncrement(newsArticleId);

            if (!isIncremented)
            {
                return null;
            }

            var newsArticle = await DetailsWithNoViewCountIncrement(newsArticleId);

            return newsArticle;
        }

        public async Task<NewsArticleDetailsServiceModel> DetailsWithNoViewCountIncrement(int newsArticleId)
            => await this.data
                .NewsArticles
                .Where(na => na.Id == newsArticleId)
                .ProjectTo<NewsArticleDetailsServiceModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

        public async Task<int> Create(
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
                NewsArticleTags = CreateNewsArticleTags(tagIds),
                AuthorId = authorId
            };

            await this.data.NewsArticles.AddAsync(newsArticle);
            await this.data.SaveChangesAsync();

            return newsArticle.Id;
        }

        public async Task<bool> Edit(
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
            newsArticle.NewsCategoryId = categoryId;
            newsArticle.NewsArticleTags = CreateNewsArticleTags(tagIds);

            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(int newsArticleId)
        {
            var newsArticle = await this.data
                .NewsArticles
                .Include(na => na.NewsArticleTags)
                .FirstOrDefaultAsync(na => na.Id == newsArticleId);

            if (newsArticle == null)
            {
                return false;
            }

            newsArticle.NewsArticleTags = null;

            this.data.NewsArticles.Remove(newsArticle);
            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<bool> IsByAuthor(int newsArticleId, int authorId)
            => await this.data
                .NewsArticles
                .AnyAsync(na => na.Id == newsArticleId && na.AuthorId == authorId);

        public async Task<IEnumerable<CategoryServiceModel>> Categories()
            => await this.data
                .NewsCategories
                .ProjectTo<CategoryServiceModel>(this.mapper.ConfigurationProvider)
                .OrderBy(c => c.Name)
                .ToListAsync();

        public async Task<IEnumerable<string>> CategoryNames()
            => await this.data
                .NewsCategories
                .Select(nc => nc.Name)
                .OrderBy(name => name)
                .ToListAsync();

        public async Task<bool> CategoryExists(int categoryId)
            => await this.data
                .NewsCategories
                .AnyAsync(nc => nc.Id == categoryId);

        public async Task<IEnumerable<TagServiceModel>> Tags()
            => await this.data
                .Tags
                .ProjectTo<TagServiceModel>(this.mapper.ConfigurationProvider)
                .OrderBy(t => t.Name)
                .ToListAsync();

        public async Task<IEnumerable<string>> TagNames()
            => await this.data
                .Tags
                .Select(t => t.Name.ToLower())
                .OrderBy(name => name)
                .ToListAsync();

        public bool TagsExist(IEnumerable<int> tagIds)
            => tagIds.All(tagId => TagExists(tagId).GetAwaiter().GetResult());

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

        private async Task<bool> TagExists(int tagId)
            => await this.data
                .Tags
                .AnyAsync(t => t.Id == tagId);

        private async Task<IEnumerable<NewsArticleServiceModel>> GetNewsArticles(IQueryable<NewsArticle> newsQuery)
            => await newsQuery
                .ProjectTo<NewsArticleServiceModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        private async Task<bool> ViewCountIncrement(int newsArticleId)
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
