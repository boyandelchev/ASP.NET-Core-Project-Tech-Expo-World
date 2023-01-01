namespace TechExpoWorld.Services.Attendees
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TechExpoWorld.Services.Attendees.Models;

    public interface IAttendeeService
    {
        Task<bool> IsAttendee(string userId);

        Task<string> AttendeeId(string userId);

        Task<string> Create(
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
            string userId);

        Task<IEnumerable<JobTypeServiceModel>> JobTypes();

        Task<bool> JobTypeExists(int jobTypeId);

        Task<IEnumerable<CompanyTypeServiceModel>> CompanyTypes();

        Task<bool> CompanyTypeExists(int companyTypeId);

        Task<IEnumerable<CompanySectorServiceModel>> CompanySectors();

        Task<bool> CompanySectorExists(int companySectorId);

        Task<IEnumerable<CompanySizeServiceModel>> CompanySizes();

        Task<bool> CompanySizeExists(int companySizeId);
    }
}
