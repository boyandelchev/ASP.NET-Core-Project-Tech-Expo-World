namespace TechExpoWorld.Services.Data.Attendees
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using TechExpoWorld.Data.Common.Repositories;
    using TechExpoWorld.Data.Models;
    using TechExpoWorld.Services.Mapping;

    public class AttendeesService : IAttendeesService
    {
        private readonly IDeletableEntityRepository<Attendee> attendeesRepository;
        private readonly IDeletableEntityRepository<Country> countriesRepository;
        private readonly IDeletableEntityRepository<JobType> jobTypesRepository;
        private readonly IDeletableEntityRepository<CompanyType> companyTypesRepository;
        private readonly IDeletableEntityRepository<CompanySector> companySectorsRepository;
        private readonly IDeletableEntityRepository<CompanySize> companySizesRepository;

        public AttendeesService(
            IDeletableEntityRepository<Attendee> attendeesRepository,
            IDeletableEntityRepository<Country> countriesRepository,
            IDeletableEntityRepository<JobType> jobTypesRepository,
            IDeletableEntityRepository<CompanyType> companyTypesRepository,
            IDeletableEntityRepository<CompanySector> companySectorsRepository,
            IDeletableEntityRepository<CompanySize> companySizesRepository)
        {
            this.attendeesRepository = attendeesRepository;
            this.countriesRepository = countriesRepository;
            this.jobTypesRepository = jobTypesRepository;
            this.companyTypesRepository = companyTypesRepository;
            this.companySectorsRepository = companySectorsRepository;
            this.companySizesRepository = companySizesRepository;
        }

        public async Task<bool> IsAttendeeAsync(string userId)
            => await this.attendeesRepository
                .All()
                .AnyAsync(a => a.UserId == userId);

        public async Task<string> AttendeeIdAsync(string userId)
            => await this.attendeesRepository
                .All()
                .Where(a => a.UserId == userId)
                .Select(a => a.Id)
                .FirstOrDefaultAsync();

        public async Task<string> CreateAsync(
            string name,
            string phoneNumber,
            string workEmail,
            string jobTitle,
            string companyName,
            int countryId,
            int jobTypeId,
            int companyTypeId,
            int companySectorId,
            int companySizeId,
            string userId)
        {
            var attendee = new Attendee
            {
                Name = name,
                PhoneNumber = phoneNumber,
                WorkEmail = workEmail,
                JobTitle = jobTitle,
                CompanyName = companyName,
                CountryId = countryId,
                JobTypeId = jobTypeId,
                CompanyTypeId = companyTypeId,
                CompanySectorId = companySectorId,
                CompanySizeId = companySizeId,
                UserId = userId,
            };

            await this.attendeesRepository.AddAsync(attendee);
            await this.attendeesRepository.SaveChangesAsync();

            return attendee.Id;
        }

        public async Task<IEnumerable<T>> CountriesAsync<T>()
            => await this.countriesRepository
                .All()
                .To<T>()
                .ToListAsync();

        public async Task<bool> CountryExistsAsync(int countryId)
            => await this.countriesRepository
                .All()
                .AnyAsync(c => c.Id == countryId);

        public async Task<IEnumerable<T>> JobTypesAsync<T>()
            => await this.jobTypesRepository
                .All()
                .To<T>()
                .ToListAsync();

        public async Task<bool> JobTypeExistsAsync(int jobTypeId)
            => await this.jobTypesRepository
                .All()
                .AnyAsync(jt => jt.Id == jobTypeId);

        public async Task<IEnumerable<T>> CompanyTypesAsync<T>()
            => await this.companyTypesRepository
                .All()
                .To<T>()
                .ToListAsync();

        public async Task<bool> CompanyTypeExistsAsync(int companyTypeId)
            => await this.companyTypesRepository
                .All()
                .AnyAsync(ct => ct.Id == companyTypeId);

        public async Task<IEnumerable<T>> CompanySectorsAsync<T>()
            => await this.companySectorsRepository
                .All()
                .To<T>()
                .ToListAsync();

        public async Task<bool> CompanySectorExistsAsync(int companySectorId)
            => await this.companySectorsRepository
                .All()
                .AnyAsync(cs => cs.Id == companySectorId);

        public async Task<IEnumerable<T>> CompanySizesAsync<T>()
            => await this.companySizesRepository
                .All()
                .To<T>()
                .ToListAsync();

        public async Task<bool> CompanySizeExistsAsync(int companySizeId)
            => await this.companySizesRepository
                .All()
                .AnyAsync(cs => cs.Id == companySizeId);
    }
}
