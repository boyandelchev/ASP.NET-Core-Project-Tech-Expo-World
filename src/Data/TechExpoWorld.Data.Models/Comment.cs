﻿namespace TechExpoWorld.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using TechExpoWorld.Data.Common.Models;

    using static TechExpoWorld.Common.GlobalConstants.Comment;

    public class Comment : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; init; }

        public int? NewsArticleId { get; init; }

        public NewsArticle NewsArticle { get; init; }

        [Required]
        public string UserId { get; init; }

        public ApplicationUser User { get; init; }

        public int Depth { get; init; }

        public int? ParentCommentId { get; init; }

        public Comment ParentComment { get; init; }

        public IEnumerable<Comment> ChildrenComments { get; init; } = new List<Comment>();
    }
}
