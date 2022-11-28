namespace TechExpoWorld.Services.Statistics
{
    using System.Threading.Tasks;
    using TechExpoWorld.Services.Statistics.Models;

    public interface IStatisticsService
    {
        Task<StatisticsServiceModel> Total();
    }
}
