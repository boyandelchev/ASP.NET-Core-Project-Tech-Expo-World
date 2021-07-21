namespace TechExpoWorld.Models.News
{
    using System.Collections.Generic;
    using TechExpoWorld.Data.Models;

    public class AddNewsFormModel
    {
        public string Title { get; init; }

        public string Content { get; init; }

        public string ImageUrl { get; init; }

        public int NewsCategoryId { get; init; }

        public IEnumerable<NewsCategory> NewsCategories { get; set; }
    }
}
