﻿namespace TechExpoWorld.Infrastructure
{
    using System.Linq;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using TechExpoWorld.Data;
    using TechExpoWorld.Data.Models;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

            var data = scopedServices.ServiceProvider.GetService<TechExpoDbContext>();

            data.Database.Migrate();

            SeedNewsCategories(data);

            SeedTags(data);

            return app;
        }

        private static void SeedNewsCategories(TechExpoDbContext data)
        {
            if (data.NewsCategories.Any())
            {
                return;
            }

            data.NewsCategories.AddRange(new[]
            {
                new NewsCategory { Name = "AI" },
                new NewsCategory { Name = "Big Data" },
                new NewsCategory { Name = "Blockchain" },
                new NewsCategory { Name = "Cloud Computing" },
                new NewsCategory { Name = "Cybersecurity" },
                new NewsCategory { Name = "Healthcare" },
                new NewsCategory { Name = "IoT" },
                new NewsCategory { Name = "Machine Learning" },
                new NewsCategory { Name = "Smart Cities" },
                new NewsCategory { Name = "Transportation" },
            });

            data.SaveChanges();
        }

        private static void SeedTags(TechExpoDbContext data)
        {
            if (data.Tags.Any())
            {
                return;
            }

            data.Tags.AddRange(new[]
            {
                new Tag { Name = "Asia" },
                new Tag { Name = "Europe" },
                new Tag { Name = "North America" },
                new Tag { Name = "agriculture" },
                new Tag { Name = "agriculture" },
                new Tag { Name = "AI" },
                new Tag { Name = "Airlines" },
                new Tag { Name = "automotive" },
                new Tag { Name = "Big Data" },
                new Tag { Name = "Blockchain" },
                new Tag { Name = "Cloud Computing" },
                new Tag { Name = "Connected Car" },
                new Tag { Name = "Connected Industry" },
                new Tag { Name = "Connected Living" },
                new Tag { Name = "Connectivity" },
                new Tag { Name = "consumer" },
                new Tag { Name = "Cybersecurity" },
                new Tag { Name = "Data" },
                new Tag { Name = "Developer" },
                new Tag { Name = "Digital Marketing" },
                new Tag { Name = "Engineering" },
                new Tag { Name = "Enterprise" },
                new Tag { Name = "Event info" },
                new Tag { Name = "Exhibition" },
                new Tag { Name = "Featured" },
                new Tag { Name = "Global" },
                new Tag { Name = "Government" },
                new Tag { Name = "Healthcare" },
                new Tag { Name = "Industry" },
                new Tag { Name = "insurance" },
                new Tag { Name = "Interoperability" },
                new Tag { Name = "Interviews" },
                new Tag { Name = "IoT" },
                new Tag { Name = "Logistics" },
                new Tag { Name = "Machine Learning" },
                new Tag { Name = "Marketing" },
                new Tag { Name = "Media" },
                new Tag { Name = "Networking" },
                new Tag { Name = "News" },
                new Tag { Name = "Payments" },
                new Tag { Name = "Retail" },
                new Tag { Name = "security" },
                new Tag { Name = "Smart Cities" },
                new Tag { Name = "Smart Home" },
                new Tag { Name = "social insight" },
                new Tag { Name = "Social Media" },
                new Tag { Name = "Space" },
                new Tag { Name = "Speakers" },
                new Tag { Name = "Sponsor" },
                new Tag { Name = "Start-Ups" },
                new Tag { Name = "supply chain" },
                new Tag { Name = "sustainability" },
                new Tag { Name = "Telecoms" },
                new Tag { Name = "Transportation" },
                new Tag { Name = "Utilities" },
                new Tag { Name = "Wearables" },
                new Tag { Name = "Workshop" },
            });

            data.SaveChanges();
        }
    }
}