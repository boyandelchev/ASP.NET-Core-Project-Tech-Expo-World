namespace TechExpoWorld.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using TechExpoWorld.Data.Common.Models;

    using static TechExpoWorld.Common.GlobalConstants.Category;

    public class Category : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; init; }

        public IEnumerable<NewsArticle> NewsArticles { get; init; } = new List<NewsArticle>();
    }
}
