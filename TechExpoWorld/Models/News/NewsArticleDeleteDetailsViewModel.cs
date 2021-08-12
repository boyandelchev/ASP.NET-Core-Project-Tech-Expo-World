namespace TechExpoWorld.Models.News
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using TechExpoWorld.Services.News.Models;

    public class NewsArticleDeleteDetailsViewModel
    {
        public int Id { get; init; }

        public string Title { get; init; }

        public string Content { get; init; }

        [Display(Name = "Image URL")]
        public string ImageUrl { get; init; }

        [Display(Name = "Selected Category")]
        public int CategoryId { get; init; }

        [Display(Name = "Selected Tags")]
        public IEnumerable<int> TagIds { get; init; } = new List<int>();

        public IEnumerable<CategoryServiceModel> Categories { get; set; }

        public IEnumerable<TagServiceModel> Tags { get; set; }
    }
}
