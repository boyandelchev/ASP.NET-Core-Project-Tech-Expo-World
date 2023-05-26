namespace TechExpoWorld.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using TechExpoWorld.Data.Models;

    internal class CompanySectorsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.CompanySectors.AnyAsync())
            {
                return;
            }

            await dbContext.CompanySectors.AddRangeAsync(new[]
            {
                new CompanySector { Name = "Automotive/Transport/Logistics" },
                new CompanySector { Name = "Built Environment (Inc Real Estate, Construction, Facilities, and Cities)" },
                new CompanySector { Name = "Business Functions (Inc Marketing, HR)" },
                new CompanySector { Name = "Communications (Inc Telcos, 5G)" },
                new CompanySector { Name = "Consultancy/Advisor/Research" },
                new CompanySector { Name = "Consumer Goods/Retail" },
                new CompanySector { Name = "Financial Services (Inc Banking, Insurance)" },
                new CompanySector { Name = "Healthcare (Inc Pharma)" },
                new CompanySector { Name = "Investment (Inc VC/Crypto Asset/STOs/ICOs)" },
                new CompanySector { Name = "IT Services" },
                new CompanySector { Name = "Manufacturing/Supply Chain" },
                new CompanySector { Name = "Media/PR (Inc Entertainment, Tourism, Events)" },
                new CompanySector { Name = "Platforms (Inc Software, Hardware, Web, Cloud)" },
                new CompanySector { Name = "Public Sector (Inc Government, NfP, Education, Defense)" },
                new CompanySector { Name = "Regulations/Compliance/Law" },
                new CompanySector { Name = "Utilities/Energy" },
            });
        }
    }
}
