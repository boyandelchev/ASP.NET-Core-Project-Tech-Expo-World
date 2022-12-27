namespace TechExpoWorld.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static GlobalConstants.Tag;

    public class Tag
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        public IEnumerable<NewsArticleTag> NewsArticleTags { get; init; } = new List<NewsArticleTag>();
    }
}
