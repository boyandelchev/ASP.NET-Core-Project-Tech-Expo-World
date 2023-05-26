namespace TechExpoWorld.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using TechExpoWorld.Data.Models;

    internal class CompanyTypesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.CompanyTypes.AnyAsync())
            {
                return;
            }

            await dbContext.CompanyTypes.AddRangeAsync(new[]
            {
                new CompanyType { Name = "Consultancy / Advisor / Research" },
                new CompanyType { Name = "Enterprise/SME" },
                new CompanyType { Name = "Investor / VC" },
                new CompanyType { Name = "Press / Media" },
                new CompanyType { Name = "Public Sector" },
                new CompanyType { Name = "Service Provider" },
                new CompanyType { Name = "Start-up" },
                new CompanyType { Name = "System Integrator" },
                new CompanyType { Name = "Technology Solution Provider" },
            });
        }
    }
}
