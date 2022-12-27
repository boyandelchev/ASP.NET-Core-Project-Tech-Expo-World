namespace TechExpoWorld.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static GlobalConstants.Comment;

    public class Comment
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }

        public DateTime CreatedOn { get; init; } = DateTime.UtcNow;

        public DateTime? LastModifiedOn { get; set; }

        public int NewsArticleId { get; init; }

        public NewsArticle NewsArticle { get; init; }

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }

        public int? ParentCommentId { get; set; }

        public Comment ParentComment { get; set; }

        public IEnumerable<Comment> ChildrenComments { get; set; } = new List<Comment>();
    }
}
