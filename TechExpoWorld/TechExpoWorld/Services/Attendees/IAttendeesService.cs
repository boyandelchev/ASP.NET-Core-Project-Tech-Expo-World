namespace TechExpoWorld.Services.Attendees
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TechExpoWorld.Services.Attendees.Models;

    public interface IAttendeesService
    {
        Task<bool> IsAttendeeAsync(string userId);

        Task<string> AttendeeIdAsync(string userId);

        Task<string> CreateAsync(
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
            string userId);

        Task<IEnumerable<CountryServiceModel>> CountriesAsync();

        Task<bool> CountryExistsAsync(int countryId);

        Task<IEnumerable<JobTypeServiceModel>> JobTypesAsync();

        Task<bool> JobTypeExistsAsync(int jobTypeId);

        Task<IEnumerable<CompanyTypeServiceModel>> CompanyTypesAsync();

        Task<bool> CompanyTypeExistsAsync(int companyTypeId);

        Task<IEnumerable<CompanySectorServiceModel>> CompanySectorsAsync();

        Task<bool> CompanySectorExistsAsync(int companySectorId);

        Task<IEnumerable<CompanySizeServiceModel>> CompanySizesAsync();

        Task<bool> CompanySizeExistsAsync(int companySizeId);
    }
}
