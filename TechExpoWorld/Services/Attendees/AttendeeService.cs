namespace TechExpoWorld.Services.Attendees
{
    using System.Collections.Generic;
    using System.Linq;
    using TechExpoWorld.Data;
    using TechExpoWorld.Data.Models;

    public class AttendeeService : IAttendeeService
    {
        private readonly TechExpoDbContext data;

        public AttendeeService(TechExpoDbContext data)
            => this.data = data;

        public bool IsAttendee(string userId)
            => this.data
                .Attendees
                .Any(a => a.UserId == userId);

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
                .Select(jt => new JobTypeServiceModel
                {
                    Id = jt.Id,
                    Name = jt.Name
                })
                .OrderBy(jt => jt.Name)
                .ToList();

        public IEnumerable<CompanyTypeServiceModel> CompanyTypes()
            => this.data
                .CompanyTypes
                .Select(ct => new CompanyTypeServiceModel
                {
                    Id = ct.Id,
                    Name = ct.Name
                })
                .OrderBy(ct => ct.Name)
                .ToList();

        public IEnumerable<CompanySectorServiceModel> CompanySectors()
            => this.data
                .CompanySectors
                .Select(cs => new CompanySectorServiceModel
                {
                    Id = cs.Id,
                    Name = cs.Name
                })
                .OrderBy(cs => cs.Name)
                .ToList();

        public IEnumerable<CompanySizeServiceModel> CompanySizes()
            => this.data
                .CompanySizes
                .Select(cs => new CompanySizeServiceModel
                {
                    Id = cs.Id,
                    Name = cs.Name
                })
                .ToList();
    }
}
