namespace TechExpoWorld.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using TechExpoWorld.Data.Models;

    internal class CategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.Categories.AnyAsync())
            {
                return;
            }

            await dbContext.Categories.AddRangeAsync(new[]
            {
                new Category { Name = "AI" },
                new Category { Name = "Big Data" },
                new Category { Name = "Blockchain" },
                new Category { Name = "Cloud Computing" },
                new Category { Name = "Cybersecurity" },
                new Category { Name = "Healthcare" },
                new Category { Name = "IoT" },
                new Category { Name = "Machine Learning" },
                new Category { Name = "Smart Cities" },
                new Category { Name = "Transportation" },
            });
        }
    }
}
