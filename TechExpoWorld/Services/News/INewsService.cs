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

        IEnumerable<string> AllNewsCategories();

        IEnumerable<string> AllNewsTags();
    }
}
