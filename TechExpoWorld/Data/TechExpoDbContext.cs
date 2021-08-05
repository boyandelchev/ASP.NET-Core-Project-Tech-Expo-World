namespace TechExpoWorld.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using TechExpoWorld.Data.Models;

    public class TechExpoDbContext : IdentityDbContext<User>
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

        public DbSet<Comment> Comments { get; init; }

        public DbSet<Event> Events { get; init; }

        public DbSet<Attendee> Attendees { get; init; }

        public DbSet<EventAttendee> EventAttendees { get; init; }

        public DbSet<Ticket> Tickets { get; init; }

        public DbSet<JobType> JobTypes { get; init; }

        public DbSet<CompanyType> CompanyTypes { get; init; }

        public DbSet<CompanySector> CompanySectors { get; init; }

        public DbSet<CompanySize> CompanySizes { get; init; }

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
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<Author>(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Event>()
                .HasOne(e => e.User)
                .WithMany(u => u.Events)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Attendee>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<Attendee>(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Attendee>()
                .HasOne(a => a.JobType)
                .WithMany(jt => jt.Attendees)
                .HasForeignKey(a => a.JobTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Attendee>()
                .HasOne(a => a.CompanyType)
                .WithMany(ct => ct.Attendees)
                .HasForeignKey(a => a.CompanyTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Attendee>()
                .HasOne(a => a.CompanySector)
                .WithMany(cs => cs.Attendees)
                .HasForeignKey(a => a.CompanySectorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Attendee>()
                .HasOne(a => a.CompanySize)
                .WithMany(cs => cs.Attendees)
                .HasForeignKey(a => a.CompanySizeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<EventAttendee>()
                .HasKey(ea => new { ea.EventId, ea.AttendeeId });

            builder
                .Entity<EventAttendee>()
                .HasOne(ea => ea.Event)
                .WithMany(e => e.EventAttendees)
                .HasForeignKey(ea => ea.EventId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<EventAttendee>()
                .HasOne(ea => ea.Attendee)
                .WithMany(a => a.EventAttendees)
                .HasForeignKey(ea => ea.AttendeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Ticket>()
                .Property(t => t.Price)
                .HasPrecision(8, 2);

            base.OnModelCreating(builder);
        }
    }
}
