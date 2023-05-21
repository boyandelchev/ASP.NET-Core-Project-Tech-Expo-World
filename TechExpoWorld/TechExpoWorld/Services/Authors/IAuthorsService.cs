namespace TechExpoWorld.Services.Authors
{
    using System.Threading.Tasks;

    public interface IAuthorsService
    {
        Task<bool> IsAuthorAsync(string userId);

        Task<string> AuthorIdAsync(string userId);

        Task<string> CreateAsync(
            string name,
            string phoneNumber,
            string address,
            string photoUrl,
            string userId);
    }
}
