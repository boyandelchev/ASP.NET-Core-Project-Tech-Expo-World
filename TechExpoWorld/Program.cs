namespace TechExpoWorld
{
    using System;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using TechExpoWorld.Data;
    using TechExpoWorld.Data.Models;
    using TechExpoWorld.Infrastructure.Extensions;
    using TechExpoWorld.Services.Attendees;
    using TechExpoWorld.Services.Authors;
    using TechExpoWorld.Services.Comments;
    using TechExpoWorld.Services.Events;
    using TechExpoWorld.Services.News;
    using TechExpoWorld.Services.Statistics;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var services = builder.Services;

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<TechExpoDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<User>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<TechExpoDbContext>();

            services.AddAutoMapper(typeof(Program));

            services.AddMemoryCache();

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
            });

            services.AddTransient<IAttendeeService, AttendeeService>();
            services.AddTransient<IAuthorService, AuthorService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IEventService, EventService>();
            services.AddTransient<INewsService, NewsService>();
            services.AddTransient<IStatisticsService, StatisticsService>();

            var app = builder.Build();

            app.PrepareDatabase();

            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultAreaRoute();
                endpoints.MapNewsArticleDetailsControllerRoute();
                endpoints.MapEventDetailsControllerRoute();
                endpoints.MapCancelTicketsControllerRoute();
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });

            app.Run();
        }
    }
}
