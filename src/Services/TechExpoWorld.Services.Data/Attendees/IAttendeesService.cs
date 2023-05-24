namespace TechExpoWorld.Services.Data.Attendees
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

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

        Task<IEnumerable<T>> CountriesAsync<T>();

        Task<bool> CountryExistsAsync(int countryId);

        Task<IEnumerable<T>> JobTypesAsync<T>();

        Task<bool> JobTypeExistsAsync(int jobTypeId);

        Task<IEnumerable<T>> CompanyTypesAsync<T>();

        Task<bool> CompanyTypeExistsAsync(int companyTypeId);

        Task<IEnumerable<T>> CompanySectorsAsync<T>();

        Task<bool> CompanySectorExistsAsync(int companySectorId);

        Task<IEnumerable<T>> CompanySizesAsync<T>();

        Task<bool> CompanySizeExistsAsync(int companySizeId);
    }
}
