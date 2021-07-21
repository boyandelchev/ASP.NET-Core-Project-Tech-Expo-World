namespace TechExpoWorld.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.NewsArticle;

    public class NewsArticle
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public DateTime CreatedOn { get; init; } = DateTime.UtcNow;

        public DateTime? LastModifiedOn { get; set; }

        public int ViewCount { get; set; }

        public int NewsCategoryId { get; set; }

        public NewsCategory NewsCategory { get; set; }

        public int AuthorId { get; init; }

        public Author Author { get; init; }

        public IEnumerable<NewsArticleTag> NewsArticleTags { get; init; } = new List<NewsArticleTag>();
    }
}
