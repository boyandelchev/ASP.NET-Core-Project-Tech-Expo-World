namespace TechExpoWorld.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.NewsCategory;

    public class NewsCategory
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        public IEnumerable<NewsArticle> NewsArticles { get; init; } = new List<NewsArticle>();
    }
}
