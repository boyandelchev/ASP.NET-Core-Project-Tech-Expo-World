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

    using static GlobalConstants.Admin;

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
            SeedCountries(serviceProvider);
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

                    var user = new User
                    {
                        Email = AdminEmail,
                        UserName = AdminEmail
                    };

                    await userManager.CreateAsync(user, AdminPassword);

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

            data.SaveChanges();
        }

        private static void SeedCountries(IServiceProvider serviceProvider)
        {
            var data = serviceProvider.GetRequiredService<TechExpoDbContext>();

            if (data.Countries.Any())
            {
                return;
            }

            data.Countries.AddRange(new[]
            {
                new Country { Name = "Afghanistan" },
                new Country { Name = "Albania" },
                new Country { Name = "Algeria" },
                new Country { Name = "Andorra" },
                new Country { Name = "Angola" },
                new Country { Name = "Antigua and Barbuda" },
                new Country { Name = "Argentina" },
                new Country { Name = "Armenia" },
                new Country { Name = "Australia" },
                new Country { Name = "Austria" },
                new Country { Name = "Azerbaijan" },
                new Country { Name = "The Bahamas" },
                new Country { Name = "Bahrain" },
                new Country { Name = "Bangladesh" },
                new Country { Name = "Barbados" },
                new Country { Name = "Belarus" },
                new Country { Name = "Belgium" },
                new Country { Name = "Belize" },
                new Country { Name = "Benin" },
                new Country { Name = "Bhutan" },
                new Country { Name = "Bolivia" },
                new Country { Name = "Bosnia and Herzegovina" },
                new Country { Name = "Botswana" },
                new Country { Name = "Brazil" },
                new Country { Name = "Brunei" },
                new Country { Name = "Bulgaria" },
                new Country { Name = "Burkina Faso" },
                new Country { Name = "Burundi" },
                new Country { Name = "Cabo Verde" },
                new Country { Name = "Cambodia" },
                new Country { Name = "Cameroon" },
                new Country { Name = "Canada" },
                new Country { Name = "Central African Republic" },
                new Country { Name = "Chad" },
                new Country { Name = "Chile" },
                new Country { Name = "China" },
                new Country { Name = "Colombia" },
                new Country { Name = "Comoros" },
                new Country { Name = "Congo, Democratic Republic of the" },
                new Country { Name = "Congo, Republic of the" },
                new Country { Name = "Costa Rica" },
                new Country { Name = "Côte d’Ivoire" },
                new Country { Name = "Croatia" },
                new Country { Name = "Cuba" },
                new Country { Name = "Cyprus" },
                new Country { Name = "Czech Republic" },
                new Country { Name = "Denmark" },
                new Country { Name = "Djibouti" },
                new Country { Name = "Dominica" },
                new Country { Name = "Dominican Republic" },
                new Country { Name = "East Timor (Timor-Leste)" },
                new Country { Name = "Ecuador" },
                new Country { Name = "Egypt" },
                new Country { Name = "El Salvador" },
                new Country { Name = "Equatorial Guinea" },
                new Country { Name = "Eritrea" },
                new Country { Name = "Estonia" },
                new Country { Name = "Eswatini" },
                new Country { Name = "Ethiopia" },
                new Country { Name = "Fiji" },
                new Country { Name = "Finland" },
                new Country { Name = "France" },
                new Country { Name = "Gabon" },
                new Country { Name = "The Gambia" },
                new Country { Name = "Georgia" },
                new Country { Name = "Germany" },
                new Country { Name = "Ghana" },
                new Country { Name = "Greece" },
                new Country { Name = "Grenada" },
                new Country { Name = "Guatemala" },
                new Country { Name = "Guinea" },
                new Country { Name = "Guinea-Bissau" },
                new Country { Name = "Guyana" },
                new Country { Name = "Haiti" },
                new Country { Name = "Honduras" },
                new Country { Name = "Hungary" },
                new Country { Name = "Iceland" },
                new Country { Name = "India" },
                new Country { Name = "Indonesia" },
                new Country { Name = "Iran" },
                new Country { Name = "Iraq" },
                new Country { Name = "Ireland" },
                new Country { Name = "Israel" },
                new Country { Name = "Italy" },
                new Country { Name = "Jamaica" },
                new Country { Name = "Japan" },
                new Country { Name = "Jordan" },
                new Country { Name = "Kazakhstan" },
                new Country { Name = "Kenya" },
                new Country { Name = "Kiribati" },
                new Country { Name = "Korea, North" },
                new Country { Name = "Korea, South" },
                new Country { Name = "Kosovo" },
                new Country { Name = "Kuwait" },
                new Country { Name = "Kyrgyzstan" },
                new Country { Name = "Laos" },
                new Country { Name = "Latvia" },
                new Country { Name = "Lebanon" },
                new Country { Name = "Lesotho" },
                new Country { Name = "Liberia" },
                new Country { Name = "Libya" },
                new Country { Name = "Liechtenstein" },
                new Country { Name = "Lithuania" },
                new Country { Name = "Luxembourg" },
                new Country { Name = "Madagascar" },
                new Country { Name = "Malawi" },
                new Country { Name = "Malaysia" },
                new Country { Name = "Maldives" },
                new Country { Name = "Mali" },
                new Country { Name = "Malta" },
                new Country { Name = "Marshall Islands" },
                new Country { Name = "Mauritania" },
                new Country { Name = "Mauritius" },
                new Country { Name = "Mexico" },
                new Country { Name = "Micronesia, Federated States of" },
                new Country { Name = "Moldova" },
                new Country { Name = "Monaco" },
                new Country { Name = "Mongolia" },
                new Country { Name = "Montenegro" },
                new Country { Name = "Morocco" },
                new Country { Name = "Mozambique" },
                new Country { Name = "Myanmar (Burma)" },
                new Country { Name = "Namibia" },
                new Country { Name = "Nauru" },
                new Country { Name = "Nepal" },
                new Country { Name = "Netherlands" },
                new Country { Name = "New Zealand" },
                new Country { Name = "Nicaragua" },
                new Country { Name = "Niger" },
                new Country { Name = "Nigeria" },
                new Country { Name = "North Macedonia" },
                new Country { Name = "Norway" },
                new Country { Name = "Oman" },
                new Country { Name = "Pakistan" },
                new Country { Name = "Palau" },
                new Country { Name = "Panama" },
                new Country { Name = "Papua New Guinea" },
                new Country { Name = "Paraguay" },
                new Country { Name = "Peru" },
                new Country { Name = "Philippines" },
                new Country { Name = "Poland" },
                new Country { Name = "Portugal" },
                new Country { Name = "Qatar" },
                new Country { Name = "Romania" },
                new Country { Name = "Russia" },
                new Country { Name = "Rwanda" },
                new Country { Name = "Saint Kitts and Nevis" },
                new Country { Name = "Saint Lucia" },
                new Country { Name = "Saint Vincent and the Grenadines" },
                new Country { Name = "Samoa" },
                new Country { Name = "San Marino" },
                new Country { Name = "Sao Tome and Principe" },
                new Country { Name = "Saudi Arabia" },
                new Country { Name = "Senegal" },
                new Country { Name = "Serbia" },
                new Country { Name = "Seychelles" },
                new Country { Name = "Sierra Leone" },
                new Country { Name = "Singapore" },
                new Country { Name = "Slovakia" },
                new Country { Name = "Slovenia" },
                new Country { Name = "Solomon Islands" },
                new Country { Name = "Somalia" },
                new Country { Name = "South Africa" },
                new Country { Name = "Spain" },
                new Country { Name = "Sri Lanka" },
                new Country { Name = "Sudan" },
                new Country { Name = "Sudan, South" },
                new Country { Name = "Suriname" },
                new Country { Name = "Sweden" },
                new Country { Name = "Switzerland" },
                new Country { Name = "Syria" },
                new Country { Name = "Taiwan" },
                new Country { Name = "Tajikistan" },
                new Country { Name = "Tanzania" },
                new Country { Name = "Thailand" },
                new Country { Name = "Togo" },
                new Country { Name = "Tonga" },
                new Country { Name = "Trinidad and Tobago" },
                new Country { Name = "Tunisia" },
                new Country { Name = "Turkey" },
                new Country { Name = "Turkmenistan" },
                new Country { Name = "Tuvalu" },
                new Country { Name = "Uganda" },
                new Country { Name = "Ukraine" },
                new Country { Name = "United Arab Emirates" },
                new Country { Name = "United Kingdom" },
                new Country { Name = "United States" },
                new Country { Name = "Uruguay" },
                new Country { Name = "Uzbekistan" },
                new Country { Name = "Vanuatu" },
                new Country { Name = "Vatican City" },
                new Country { Name = "Venezuela" },
                new Country { Name = "Vietnam" },
                new Country { Name = "Yemen" },
                new Country { Name = "Zambia" },
                new Country { Name = "Zimbabwe" },
                new Country { Name = "Other" },
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
                new JobType { Name = "CxO" },
                new JobType { Name = "Director" },
                new JobType { Name = "Founder/Partner" },
                new JobType { Name = "Senior Manager" },
                new JobType { Name = "VP" },
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
