namespace TechExpoWorld.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using TechExpoWorld.Data.Models;

    internal class CompanySizesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.CompanySizes.AnyAsync())
            {
                return;
            }

            await dbContext.CompanySizes.AddRangeAsync(new[]
            {
                new CompanySize { Name = "1-10" },
                new CompanySize { Name = "11-200" },
                new CompanySize { Name = "201-1000" },
                new CompanySize { Name = "1001-5000" },
                new CompanySize { Name = "5000+" },
            });
        }
    }
}
