namespace TechExpoWorld.Services.News
{
    using System.Collections.Generic;
    using TechExpoWorld.Models.News;
    using TechExpoWorld.Services.News.Models;

    public interface INewsService
    {
        NewsArticlesQueryServiceModel All(
            string category,
            string tag,
            string searchTerm,
            NewsSorting sorting,
            int currentPage,
            int newsArticlesPerPage);

        IEnumerable<NewsArticleServiceModel> NewsArticlesByUser(string userId);

        IEnumerable<LatestNewsArticleServiceModel> LatestNewsArticles();

        NewsArticleDetailsServiceModel Details(int newsArticleId);

        NewsArticleDetailsServiceModel DetailsWithNoViewCountIncrement(int newsArticleId);

        int Create(
            string title,
            string content,
            string imageUrl,
            int categoryId,
            IEnumerable<int> tagIds,
            int authorId);

        bool Edit(
            int newsArticleId,
            string title,
            string content,
            string imageUrl,
            int categoryId,
            IEnumerable<int> tagIds);

        bool Delete(int newsArticleId);

        bool IsByAuthor(int newsArticleId, int authorId);

        IEnumerable<CategoryServiceModel> Categories();

        IEnumerable<string> CategoryNames();

        bool CategoryExists(int categoryId);

        IEnumerable<TagServiceModel> Tags();

        IEnumerable<string> TagNames();

        bool TagsExist(IEnumerable<int> tagIds);
    }
}
