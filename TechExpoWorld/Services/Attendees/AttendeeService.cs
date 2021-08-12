namespace TechExpoWorld.Services.Attendees
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
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

        public bool IsAttendee(string userId)
            => this.data
                .Attendees
                .Any(a => a.UserId == userId);

        public int AttendeeId(string userId)
            => this.data
                .Attendees
                .Where(a => a.UserId == userId)
                .Select(a => a.Id)
                .FirstOrDefault();

        public int Create(
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

            this.data.Attendees.Add(attendee);
            this.data.SaveChanges();

            return attendee.Id;
        }

        public IEnumerable<JobTypeServiceModel> JobTypes()
            => this.data
                .JobTypes
                .ProjectTo<JobTypeServiceModel>(this.mapper.ConfigurationProvider)
                .OrderBy(jt => jt.Name)
                .ToList();

        public bool JobTypeExists(int jobTypeId)
            => this.data
                .JobTypes
                .Any(jt => jt.Id == jobTypeId);

        public IEnumerable<CompanyTypeServiceModel> CompanyTypes()
            => this.data
                .CompanyTypes
                .ProjectTo<CompanyTypeServiceModel>(this.mapper.ConfigurationProvider)
                .OrderBy(ct => ct.Name)
                .ToList();

        public bool CompanyTypeExists(int companyTypeId)
            => this.data
                .CompanyTypes
                .Any(ct => ct.Id == companyTypeId);

        public IEnumerable<CompanySectorServiceModel> CompanySectors()
            => this.data
                .CompanySectors
                .ProjectTo<CompanySectorServiceModel>(this.mapper.ConfigurationProvider)
                .OrderBy(cs => cs.Name)
                .ToList();

        public bool CompanySectorExists(int companySectorId)
            => this.data
                .CompanySectors
                .Any(cs => cs.Id == companySectorId);

        public IEnumerable<CompanySizeServiceModel> CompanySizes()
            => this.data
                .CompanySizes
                .ProjectTo<CompanySizeServiceModel>(this.mapper.ConfigurationProvider)
                .ToList();

        public bool CompanySizeExists(int companySizeId)
            => this.data
                .CompanySizes
                .Any(cs => cs.Id == companySizeId);
    }
}
