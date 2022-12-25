namespace TechExpoWorld.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TechExpoWorld.Data.Models;

    public class AttendeeConfiguration : IEntityTypeConfiguration<Attendee>
    {
        public void Configure(EntityTypeBuilder<Attendee> builder)
        {
            builder
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<Attendee>(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(a => a.JobType)
                .WithMany(jt => jt.Attendees)
                .HasForeignKey(a => a.JobTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(a => a.CompanyType)
                .WithMany(ct => ct.Attendees)
                .HasForeignKey(a => a.CompanyTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(a => a.CompanySector)
                .WithMany(cs => cs.Attendees)
                .HasForeignKey(a => a.CompanySectorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(a => a.CompanySize)
                .WithMany(cs => cs.Attendees)
                .HasForeignKey(a => a.CompanySizeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
