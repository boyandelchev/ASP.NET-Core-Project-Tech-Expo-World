namespace TechExpoWorld.Services.News
{
    using System.Collections.Generic;
    using TechExpoWorld.Models.News;

    public interface INewsService
    {
        NewsArticlesQueryServiceModel All(
            string category,
            string tag,
            string searchTerm,
            NewsSorting sorting,
            int currentPage,
            int newsArticlesPerPage);

        List<NewsArticleIndexServiceModel> LatestNewsArticles();

        int Create(
            string title,
            string content,
            string imageUrl,
            int newsCategoryId,
            int authorId,
            IEnumerable<int> tagIds);

        IEnumerable<CategoryServiceModel> Categories();

        IEnumerable<string> CategoryNames();

        bool CategoryExists(int categoryId);

        IEnumerable<TagServiceModel> Tags();

        IEnumerable<string> TagNames();

        bool TagExists(int tagId);
    }
}
