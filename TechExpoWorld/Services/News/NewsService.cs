namespace TechExpoWorld.Services.News
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using TechExpoWorld.Data;
    using TechExpoWorld.Data.Models;
    using TechExpoWorld.Models.News;

    public class NewsService : INewsService
    {
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

            var news = newsQuery
                .Skip((currentPage - 1) * newsArticlesPerPage)
                .Take(newsArticlesPerPage)
                .Select(na => new NewsArticleServiceModel
                {
                    Id = na.Id,
                    Title = na.Title,
                    Content = na.Content.Substring(0, 200) + "...",
                    ImageUrl = na.ImageUrl,
                    CreatedOn = na.CreatedOn.ToString("dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture),
                    NewsCategory = na.NewsCategory.Name,
                    Author = na.Author.Name
                })
                .ToList();

            return new NewsArticlesQueryServiceModel
            {
                TotalNewsArticles = totalNewsArticles,
                News = news
            };
        }

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

        public int Create(
            string title,
            string content,
            string imageUrl,
            int newsCategoryId,
            int authorId,
            IEnumerable<int> tagIds)
        {
            var newsData = new NewsArticle
            {
                Title = title,
                Content = content,
                ImageUrl = imageUrl,
                NewsCategoryId = newsCategoryId,
                AuthorId = authorId
            };

            if (tagIds.Any())
            {
                var newsArticleTags = new List<NewsArticleTag>();

                foreach (var tagId in tagIds)
                {
                    newsArticleTags.Add(new NewsArticleTag { TagId = tagId });
                }

                newsData.NewsArticleTags = newsArticleTags;
            }

            this.data.NewsArticles.Add(newsData);
            this.data.SaveChanges();

            return newsData.Id;
        }

        public IEnumerable<CategoryServiceModel> Categories()
            => this.data
                .NewsCategories
                .Select(c => new CategoryServiceModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
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
                .ToList();

        public IEnumerable<string> TagNames()
            => this.data
                .Tags
                .Select(t => t.Name.ToLower())
                .OrderBy(name => name)
                .ToList();

        public bool TagExists(int tagId)
            => this.data
                .Tags
                .Any(t => t.Id == tagId);
    }
}
