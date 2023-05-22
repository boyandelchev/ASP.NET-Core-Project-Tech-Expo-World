namespace TechExpoWorld.Data.Models
{
    using System;

    using Microsoft.EntityFrameworkCore;

    using TechExpoWorld.Data.Common.Models;

    [PrimaryKey(nameof(NewsArticleId), nameof(TagId))]
    public class NewsArticleTag : IAuditInfo, IDeletableEntity
    {
        public int NewsArticleId { get; set; }

        public NewsArticle NewsArticle { get; set; }

        public int TagId { get; set; }

        public Tag Tag { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
