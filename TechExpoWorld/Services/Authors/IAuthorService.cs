namespace TechExpoWorld.Services.Authors
{
    using System.Threading.Tasks;

    public interface IAuthorService
    {
        Task<bool> IsAuthor(string userId);

        Task<string> AuthorId(string userId);

        Task<string> Create(
            string name,
            string phoneNumber,
            string address,
            string photoUrl,
            string userId);
    }
}
