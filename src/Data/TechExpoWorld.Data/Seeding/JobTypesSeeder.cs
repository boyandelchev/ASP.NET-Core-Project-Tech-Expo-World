namespace TechExpoWorld.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using TechExpoWorld.Data.Models;

    internal class JobTypesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.JobTypes.AnyAsync())
            {
                return;
            }

            await dbContext.JobTypes.AddRangeAsync(new[]
            {
                new JobType { Name = "CxO" },
                new JobType { Name = "Director" },
                new JobType { Name = "Founder/Partner" },
                new JobType { Name = "Senior Manager" },
                new JobType { Name = "VP" },
                new JobType { Name = "Other" },
            });
        }
    }
}
