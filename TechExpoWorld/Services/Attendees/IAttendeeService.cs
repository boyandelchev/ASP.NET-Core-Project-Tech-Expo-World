namespace TechExpoWorld.Services.Attendees
{
    using System.Collections.Generic;

    public interface IAttendeeService
    {
        bool IsAttendee(string userId);

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

        IEnumerable<CompanyTypeServiceModel> CompanyTypes();

        IEnumerable<CompanySectorServiceModel> CompanySectors();

        IEnumerable<CompanySizeServiceModel> CompanySizes();
    }
}
