namespace TechExpoWorld.Services.News
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TechExpoWorld.Services.News.Models;

    public interface INewsService
    {
        Task<NewsArticlesQueryServiceModel> AllAsync(
            string category,
            string tag,
            string searchTerm,
            NewsSorting sorting,
            int currentPage,
            int newsArticlesPerPage);

        Task<IEnumerable<NewsArticleServiceModel>> NewsArticlesByAuthorAsync(string authorId);

        Task<IList<LatestNewsArticleServiceModel>> LatestNewsArticlesAsync();

        Task<NewsArticleDetailsServiceModel> DetailsAsync(int newsArticleId);

        Task<NewsArticleDetailsServiceModel> DetailsWithNoViewCountIncrementAsync(int newsArticleId);

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

        Task<IEnumerable<CategoryServiceModel>> CategoriesAsync();

        Task<IEnumerable<string>> CategoryNamesAsync();

        Task<bool> CategoryExistsAsync(int categoryId);

        Task<IEnumerable<TagServiceModel>> TagsAsync();

        Task<IEnumerable<string>> TagNamesAsync();

        bool TagsExist(IEnumerable<int> tagIds);
    }
}
