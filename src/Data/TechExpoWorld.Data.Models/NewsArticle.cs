namespace TechExpoWorld.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using TechExpoWorld.Data.Common.Models;

    using static TechExpoWorld.Common.GlobalConstants.NewsArticle;

    public class NewsArticle : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public int ViewCount { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        [Required]
        [MaxLength(AuthorIdMaxLength)]
        public string AuthorId { get; init; }

        public Author Author { get; init; }

        public IEnumerable<NewsArticleTag> NewsArticleTags { get; set; } = new List<NewsArticleTag>();

        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();
    }
}
