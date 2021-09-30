namespace TechExpoWorld.Infrastructure.Extensions
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using TechExpoWorld.Data;
    using TechExpoWorld.Data.Models;

    using static Areas.Admin.AdminConstants;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var serviceProvider = serviceScope.ServiceProvider;

            MigrateDatabase(serviceProvider);

            SeedAdministrator(serviceProvider);
            SeedNewsCategories(serviceProvider);
            SeedTags(serviceProvider);
            SeedJobTypes(serviceProvider);
            SeedCompanyTypes(serviceProvider);
            SeedCompanySectors(serviceProvider);
            SeedCompanySizes(serviceProvider);

            return app;
        }

        private static void MigrateDatabase(IServiceProvider serviceProvider)
        {
            var data = serviceProvider.GetRequiredService<TechExpoDbContext>();

            data.Database.Migrate();
        }

        private static void SeedAdministrator(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(AdministratorRoleName))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = AdministratorRoleName };

                    await roleManager.CreateAsync(role);

                    const string adminEmail = "admin@tew.com";
                    const string adminPassword = "admin1";

                    var user = new User
                    {
                        Email = adminEmail,
                        UserName = adminEmail
                    };

                    await userManager.CreateAsync(user, adminPassword);

                    await userManager.AddToRoleAsync(user, role.Name);
                })
                .GetAwaiter()
                .GetResult();
        }

        private static void SeedNewsCategories(IServiceProvider serviceProvider)
        {
            var data = serviceProvider.GetRequiredService<TechExpoDbContext>();

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

        private static void SeedTags(IServiceProvider serviceProvider)
        {
            var data = serviceProvider.GetRequiredService<TechExpoDbContext>();

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

        private static void SeedJobTypes(IServiceProvider serviceProvider)
        {
            var data = serviceProvider.GetRequiredService<TechExpoDbContext>();

            if (data.JobTypes.Any())
            {
                return;
            }

            data.JobTypes.AddRange(new[]
            {
                new JobType { Name = "Founder/Partner" },
                new JobType { Name = "CxO" },
                new JobType { Name = "Director" },
                new JobType { Name = "VP" },
                new JobType { Name = "Senior Manager" },
                new JobType { Name = "Other" },
            });

            data.SaveChanges();
        }

        private static void SeedCompanyTypes(IServiceProvider serviceProvider)
        {
            var data = serviceProvider.GetRequiredService<TechExpoDbContext>();

            if (data.CompanyTypes.Any())
            {
                return;
            }

            data.CompanyTypes.AddRange(new[]
            {
                new CompanyType { Name = "Enterprise/SME" },
                new CompanyType { Name = "Start-up" },
                new CompanyType { Name = "Investor / VC" },
                new CompanyType { Name = "Public Sector" },
                new CompanyType { Name = "Consultancy / Advisor / Research" },
                new CompanyType { Name = "Press / Media" },
                new CompanyType { Name = "System Integrator" },
                new CompanyType { Name = "Technology Solution Provider" },
                new CompanyType { Name = "Service Provider" },
            });

            data.SaveChanges();
        }

        private static void SeedCompanySectors(IServiceProvider serviceProvider)
        {
            var data = serviceProvider.GetRequiredService<TechExpoDbContext>();

            if (data.CompanySectors.Any())
            {
                return;
            }

            data.CompanySectors.AddRange(new[]
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

            data.SaveChanges();
        }

        private static void SeedCompanySizes(IServiceProvider serviceProvider)
        {
            var data = serviceProvider.GetRequiredService<TechExpoDbContext>();

            if (data.CompanySizes.Any())
            {
                return;
            }

            data.CompanySizes.AddRange(new[]
            {
                new CompanySize { Name = "1-10" },
                new CompanySize { Name = "11-200" },
                new CompanySize { Name = "201-1000" },
                new CompanySize { Name = "1001-5000" },
                new CompanySize { Name = "5000+" },
            });

            data.SaveChanges();
        }
    }
}
