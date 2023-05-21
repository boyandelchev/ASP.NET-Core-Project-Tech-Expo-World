namespace TechExpoWorld.Services.Statistics
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using TechExpoWorld.Data;
    using TechExpoWorld.Services.Statistics.Models;

    public class StatisticsService : IStatisticsService
    {
        private readonly TechExpoDbContext data;

        public StatisticsService(TechExpoDbContext data)
            => this.data = data;

        public async Task<StatisticsServiceModel> TotalAsync()
        {
            var totalNewsArticles = await this.data.NewsArticles.CountAsync();
            var totalUsers = await this.data.Users.CountAsync();
            var totalAuthors = await this.data.Authors.CountAsync();
            var totalAttendees = await this.data.Attendees.CountAsync();
            var totalEvents = await this.data.Events.CountAsync();
            var totalLocations = await this.data.Events.Select(e => e.Location).Distinct().CountAsync();

            return new StatisticsServiceModel
            {
                TotalNewsArticles = totalNewsArticles,
                TotalUsers = totalUsers,
                TotalAuthors = totalAuthors,
                TotalAttendees = totalAttendees,
                TotalEvents = totalEvents,
                TotalLocations = totalLocations
            };
        }
    }
}
