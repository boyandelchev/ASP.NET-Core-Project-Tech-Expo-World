namespace TechExpoWorld.Services.News
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TechExpoWorld.Models.News;
    using TechExpoWorld.Services.News.Models;

    public interface INewsService
    {
        Task<NewsArticlesQueryServiceModel> All(
            string category,
            string tag,
            string searchTerm,
            NewsSorting sorting,
            int currentPage,
            int newsArticlesPerPage);

        Task<IEnumerable<NewsArticleServiceModel>> NewsArticlesByUser(string userId);

        Task<IList<LatestNewsArticleServiceModel>> LatestNewsArticles();

        Task<NewsArticleDetailsServiceModel> Details(int newsArticleId);

        Task<NewsArticleDetailsServiceModel> DetailsWithNoViewCountIncrement(int newsArticleId);

        Task<int> Create(
            string title,
            string content,
            string imageUrl,
            int categoryId,
            IEnumerable<int> tagIds,
            int authorId);

        Task<bool> Edit(
            int newsArticleId,
            string title,
            string content,
            string imageUrl,
            int categoryId,
            IEnumerable<int> tagIds);

        Task<bool> Delete(int newsArticleId);

        Task<bool> IsByAuthor(int newsArticleId, int authorId);

        Task<IEnumerable<CategoryServiceModel>> Categories();

        Task<IEnumerable<string>> CategoryNames();

        Task<bool> CategoryExists(int categoryId);

        Task<IEnumerable<TagServiceModel>> Tags();

        Task<IEnumerable<string>> TagNames();

        bool TagsExist(IEnumerable<int> tagIds);
    }
}
