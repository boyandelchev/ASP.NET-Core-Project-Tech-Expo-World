namespace TechExpoWorld.Services.Statistics
{
    using System.Linq;
    using TechExpoWorld.Data;

    public class StatisticsService : IStatisticsService
    {
        private readonly TechExpoDbContext data;

        public StatisticsService(TechExpoDbContext data)
            => this.data = data;

        public StatisticsServiceModel Total()
        {
            var totalNewsArticles = this.data.NewsArticles.Count();
            var totalUsers = this.data.Users.Count();
            var totalAuthors = this.data.Authors.Count();
            var totalEvents = this.data.Events.Count();
            var totalLocations = this.data.Events.Select(e => e.Location).Distinct().Count();

            return new StatisticsServiceModel
            {
                TotalNewsArticles = totalNewsArticles,
                TotalUsers = totalUsers,
                TotalAuthors = totalAuthors,
                TotalEvents = totalEvents,
                TotalLocations = totalLocations
            };
        }
    }
}
