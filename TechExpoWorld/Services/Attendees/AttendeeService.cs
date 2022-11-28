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

    public class AttendeeService : IAttendeeService
    {
        private readonly TechExpoDbContext data;
        private readonly IMapper mapper;

        public AttendeeService(TechExpoDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

        public async Task<bool> IsAttendee(string userId)
            => await this.data
                .Attendees
                .AnyAsync(a => a.UserId == userId);

        public async Task<int> AttendeeId(string userId)
            => await this.data
                .Attendees
                .Where(a => a.UserId == userId)
                .Select(a => a.Id)
                .FirstOrDefaultAsync();

        public async Task<int> Create(
            string name,
            string phoneNumber,
            string workEmail,
            string jobTitle,
            string companyName,
            string country,
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
                Country = country,
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

        public async Task<IEnumerable<JobTypeServiceModel>> JobTypes()
            => await this.data
                .JobTypes
                .ProjectTo<JobTypeServiceModel>(this.mapper.ConfigurationProvider)
                .OrderBy(jt => jt.Name)
                .ToListAsync();

        public async Task<bool> JobTypeExists(int jobTypeId)
            => await this.data
                .JobTypes
                .AnyAsync(jt => jt.Id == jobTypeId);

        public async Task<IEnumerable<CompanyTypeServiceModel>> CompanyTypes()
            => await this.data
                .CompanyTypes
                .ProjectTo<CompanyTypeServiceModel>(this.mapper.ConfigurationProvider)
                .OrderBy(ct => ct.Name)
                .ToListAsync();

        public async Task<bool> CompanyTypeExists(int companyTypeId)
            => await this.data
                .CompanyTypes
                .AnyAsync(ct => ct.Id == companyTypeId);

        public async Task<IEnumerable<CompanySectorServiceModel>> CompanySectors()
            => await this.data
                .CompanySectors
                .ProjectTo<CompanySectorServiceModel>(this.mapper.ConfigurationProvider)
                .OrderBy(cs => cs.Name)
                .ToListAsync();

        public async Task<bool> CompanySectorExists(int companySectorId)
            => await this.data
                .CompanySectors
                .AnyAsync(cs => cs.Id == companySectorId);

        public async Task<IEnumerable<CompanySizeServiceModel>> CompanySizes()
            => await this.data
                .CompanySizes
                .ProjectTo<CompanySizeServiceModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<bool> CompanySizeExists(int companySizeId)
            => await this.data
                .CompanySizes
                .AnyAsync(cs => cs.Id == companySizeId);
    }
}
