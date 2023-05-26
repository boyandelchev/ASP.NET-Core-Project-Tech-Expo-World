namespace TechExpoWorld.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using TechExpoWorld.Data.Models;

    internal class TagsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.Tags.AnyAsync())
            {
                return;
            }

            await dbContext.Tags.AddRangeAsync(new[]
            {
                new Tag { Name = "Agriculture" },
                new Tag { Name = "AI" },
                new Tag { Name = "Airlines" },
                new Tag { Name = "Asia" },
                new Tag { Name = "Automotive" },
                new Tag { Name = "Big Data" },
                new Tag { Name = "Blockchain" },
                new Tag { Name = "Cloud Computing" },
                new Tag { Name = "Connected Car" },
                new Tag { Name = "Connected Industry" },
                new Tag { Name = "Connected Living" },
                new Tag { Name = "Connectivity" },
                new Tag { Name = "Consumer" },
                new Tag { Name = "Cybersecurity" },
                new Tag { Name = "Data" },
                new Tag { Name = "Developer" },
                new Tag { Name = "Digital Marketing" },
                new Tag { Name = "Engineering" },
                new Tag { Name = "Enterprise" },
                new Tag { Name = "Europe" },
                new Tag { Name = "Event Info" },
                new Tag { Name = "Exhibition" },
                new Tag { Name = "Featured" },
                new Tag { Name = "Global" },
                new Tag { Name = "Government" },
                new Tag { Name = "Healthcare" },
                new Tag { Name = "Industry" },
                new Tag { Name = "Insurance" },
                new Tag { Name = "Interoperability" },
                new Tag { Name = "Interviews" },
                new Tag { Name = "IoT" },
                new Tag { Name = "Logistics" },
                new Tag { Name = "Machine Learning" },
                new Tag { Name = "Marketing" },
                new Tag { Name = "Media" },
                new Tag { Name = "Networking" },
                new Tag { Name = "News" },
                new Tag { Name = "North America" },
                new Tag { Name = "Payments" },
                new Tag { Name = "Retail" },
                new Tag { Name = "Security" },
                new Tag { Name = "Smart Cities" },
                new Tag { Name = "Smart Home" },
                new Tag { Name = "Social Insight" },
                new Tag { Name = "Social Media" },
                new Tag { Name = "Space" },
                new Tag { Name = "Speakers" },
                new Tag { Name = "Sponsor" },
                new Tag { Name = "Start-Ups" },
                new Tag { Name = "Supply Chain" },
                new Tag { Name = "Sustainability" },
                new Tag { Name = "Telecoms" },
                new Tag { Name = "Transportation" },
                new Tag { Name = "Utilities" },
                new Tag { Name = "Wearables" },
                new Tag { Name = "Workshop" },
            });
        }
    }
}
