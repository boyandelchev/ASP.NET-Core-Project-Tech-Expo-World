namespace TechExpoWorld.Models.News
{
    public class NewsArticleListingViewModel
    {
        public int Id { get; init; }

        public string Title { get; init; }

        public string Content { get; init; }

        public string ImageUrl { get; init; }

        public string CreatedOn { get; init; }

        public string NewsCategory { get; init; }

        public string Author { get; init; }
    }
}
