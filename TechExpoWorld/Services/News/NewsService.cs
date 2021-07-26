namespace TechExpoWorld.Services.News
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using TechExpoWorld.Data;
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

        public IEnumerable<string> AllNewsCategories()
            => this.data
                .NewsCategories
                .Select(nc => nc.Name)
                .OrderBy(name => name)
                .ToList();

        public IEnumerable<string> AllNewsTags()
            => this.data
                .Tags
                .Select(t => t.Name.ToLower())
                .OrderBy(name => name)
                .ToList();
    }
}
