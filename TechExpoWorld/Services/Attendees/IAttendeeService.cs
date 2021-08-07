namespace TechExpoWorld.Services.Attendees
{
    using System.Collections.Generic;

    public interface IAttendeeService
    {
        bool IsAttendee(string userId);

        int AttendeeId(string userId);

        int Create(
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

        IEnumerable<JobTypeServiceModel> JobTypes();

        bool JobTypeExists(int jobTypeId);

        IEnumerable<CompanyTypeServiceModel> CompanyTypes();

        bool CompanyTypeExists(int companyTypeId);

        IEnumerable<CompanySectorServiceModel> CompanySectors();

        bool CompanySectorExists(int companySectorId);

        IEnumerable<CompanySizeServiceModel> CompanySizes();

        bool CompanySizeExists(int companySizeId);
    }
}
