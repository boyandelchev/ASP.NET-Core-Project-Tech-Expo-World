namespace TechExpoWorld.Services.Attendees
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Microsoft.EntityFrameworkCore;

    using TechExpoWorld.Data;
    using TechExpoWorld.Data.Models;
    using TechExpoWorld.Services.Attendees.Models;

    public class AttendeesService : IAttendeesService
    {
        private readonly TechExpoDbContext data;
        private readonly IMapper mapper;

        public AttendeesService(TechExpoDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

        public async Task<bool> IsAttendeeAsync(string userId)
            => await this.data
                .Attendees
                .AnyAsync(a => a.UserId == userId);

        public async Task<string> AttendeeIdAsync(string userId)
            => await this.data
                .Attendees
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
                UserId = userId
            };

            await this.data.Attendees.AddAsync(attendee);
            await this.data.SaveChangesAsync();

            return attendee.Id;
        }

        public async Task<IEnumerable<CountryServiceModel>> CountriesAsync()
            => await this.data
                .Countries
                .ProjectTo<CountryServiceModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<bool> CountryExistsAsync(int countryId)
            => await this.data
                .Countries
                .AnyAsync(c => c.Id == countryId);

        public async Task<IEnumerable<JobTypeServiceModel>> JobTypesAsync()
            => await this.data
                .JobTypes
                .ProjectTo<JobTypeServiceModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<bool> JobTypeExistsAsync(int jobTypeId)
            => await this.data
                .JobTypes
                .AnyAsync(jt => jt.Id == jobTypeId);

        public async Task<IEnumerable<CompanyTypeServiceModel>> CompanyTypesAsync()
            => await this.data
                .CompanyTypes
                .ProjectTo<CompanyTypeServiceModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<bool> CompanyTypeExistsAsync(int companyTypeId)
            => await this.data
                .CompanyTypes
                .AnyAsync(ct => ct.Id == companyTypeId);

        public async Task<IEnumerable<CompanySectorServiceModel>> CompanySectorsAsync()
            => await this.data
                .CompanySectors
                .ProjectTo<CompanySectorServiceModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<bool> CompanySectorExistsAsync(int companySectorId)
            => await this.data
                .CompanySectors
                .AnyAsync(cs => cs.Id == companySectorId);

        public async Task<IEnumerable<CompanySizeServiceModel>> CompanySizesAsync()
            => await this.data
                .CompanySizes
                .ProjectTo<CompanySizeServiceModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<bool> CompanySizeExistsAsync(int companySizeId)
            => await this.data
                .CompanySizes
                .AnyAsync(cs => cs.Id == companySizeId);
    }
}
