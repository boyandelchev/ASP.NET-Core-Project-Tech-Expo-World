namespace TechExpoWorld.Data.Models
{
    using System;

    using Microsoft.EntityFrameworkCore;

    using TechExpoWorld.Data.Common.Models;

    [PrimaryKey(nameof(NewsArticleId), nameof(TagId))]
    public class NewsArticleTag : IAuditInfo, IDeletableEntity
    {
        public int NewsArticleId { get; init; }

        public NewsArticle NewsArticle { get; init; }

        public int TagId { get; init; }

        public Tag Tag { get; init; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
