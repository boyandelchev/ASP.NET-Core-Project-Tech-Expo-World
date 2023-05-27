namespace TechExpoWorld.Services.Data.News
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface INewsService
    {
        Task<(IEnumerable<T> NewsArticles, int TotalNewsArticles)> AllAsync<T>(
            string category,
            string tag,
            string searchTerm,
            int sorting,
            int currentPage,
            int newsArticlesPerPage);

        Task<IEnumerable<T>> NewsArticlesByAuthorAsync<T>(string authorId);

        Task<IList<T>> LatestNewsArticlesAsync<T>();

        Task<T> DetailsAsync<T>(int newsArticleId);

        Task<T> DetailsWithNoViewCountIncrementAsync<T>(int newsArticleId);

        Task<int> CreateAsync(
            string title,
            string content,
            string imageUrl,
            int categoryId,
            IEnumerable<int> tagIds,
            string authorId);

        Task<bool> EditAsync(
            int newsArticleId,
            string title,
            string content,
            string imageUrl,
            int categoryId,
            IEnumerable<int> tagIds);

        Task<bool> DeleteAsync(int newsArticleId);

        Task<bool> IsByAuthorAsync(int newsArticleId, string authorId);

        Task<IEnumerable<T>> CategoriesAsync<T>();

        Task<IEnumerable<string>> CategoryNamesAsync();

        Task<bool> CategoryExistsAsync(int categoryId);

        Task<IEnumerable<T>> TagsAsync<T>();

        Task<IEnumerable<string>> TagNamesAsync();

        bool TagsExist(IEnumerable<int> tagIds);
    }
}
