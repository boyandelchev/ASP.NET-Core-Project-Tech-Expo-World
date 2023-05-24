namespace TechExpoWorld.Services.Data.Statistics
{
    using System.Threading.Tasks;

    public interface IStatisticsService
    {
        Task<int> TotalNewsArticlesAsync();

        Task<int> TotalUsersAsync();

        Task<int> TotalAuthorsAsync();

        Task<int> TotalAttendeesAsync();

        Task<int> TotalEventsAsync();

        Task<int> TotalLocationsAsync();
    }
}
