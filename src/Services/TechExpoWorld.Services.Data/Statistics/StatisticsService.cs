namespace TechExpoWorld.Services.Data.Statistics
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using TechExpoWorld.Data.Common.Repositories;
    using TechExpoWorld.Data.Models;

    public class StatisticsService : IStatisticsService
    {
        private readonly IDeletableEntityRepository<NewsArticle> newsArticlesRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> applicationUsersRepository;
        private readonly IDeletableEntityRepository<Author> authorsRepository;
        private readonly IDeletableEntityRepository<Attendee> attendeesRepository;
        private readonly IDeletableEntityRepository<Event> eventsRepository;

        public StatisticsService(
            IDeletableEntityRepository<NewsArticle> newsArticlesRepository,
            IDeletableEntityRepository<ApplicationUser> applicationUsersRepository,
            IDeletableEntityRepository<Author> authorsRepository,
            IDeletableEntityRepository<Attendee> attendeesRepository,
            IDeletableEntityRepository<Event> eventsRepository)
        {
            this.newsArticlesRepository = newsArticlesRepository;
            this.applicationUsersRepository = applicationUsersRepository;
            this.authorsRepository = authorsRepository;
            this.attendeesRepository = attendeesRepository;
            this.eventsRepository = eventsRepository;
        }

        public async Task<int> TotalNewsArticlesAsync()
            => await this.newsArticlesRepository.All().CountAsync();

        public async Task<int> TotalUsersAsync()
            => await this.applicationUsersRepository.All().CountAsync();

        public async Task<int> TotalAuthorsAsync()
            => await this.authorsRepository.All().CountAsync();

        public async Task<int> TotalAttendeesAsync()
            => await this.attendeesRepository.All().CountAsync();

        public async Task<int> TotalEventsAsync()
            => await this.eventsRepository.All().CountAsync();

        public async Task<int> TotalLocationsAsync()
            => await this.eventsRepository
                .All()
                .Select(e => e.Location)
                .Distinct()
                .CountAsync();
    }
}
