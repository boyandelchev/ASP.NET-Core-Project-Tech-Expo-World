namespace TechExpoWorld.Data
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using TechExpoWorld.Data.Models;

    public class TechExpoDbContext : IdentityDbContext
    {
        public TechExpoDbContext(DbContextOptions<TechExpoDbContext> options)
            : base(options)
        {
        }

        public DbSet<NewsArticle> NewsArticles { get; init; }

        public DbSet<NewsCategory> NewsCategories { get; init; }

        public DbSet<Tag> Tags { get; init; }

        public DbSet<NewsArticleTag> NewsArticleTags { get; init; }

        public DbSet<Author> Authors { get; init; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<NewsArticle>()
                .HasOne(na => na.NewsCategory)
                .WithMany(nc => nc.NewsArticles)
                .HasForeignKey(na => na.NewsCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<NewsArticle>()
                .HasOne(na => na.Author)
                .WithMany(a => a.NewsArticles)
                .HasForeignKey(na => na.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<NewsArticleTag>()
                .HasKey(nt => new { nt.NewsArticleId, nt.TagId });

            builder
                .Entity<NewsArticleTag>()
                .HasOne(nat => nat.NewsArticle)
                .WithMany(na => na.NewsArticleTags)
                .HasForeignKey(nat => nat.NewsArticleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<NewsArticleTag>()
                .HasOne(nat => nat.Tag)
                .WithMany(t => t.NewsArticleTags)
                .HasForeignKey(nat => nat.TagId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Author>()
                .HasOne<IdentityUser>()
                .WithOne()
                .HasForeignKey<Author>(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
