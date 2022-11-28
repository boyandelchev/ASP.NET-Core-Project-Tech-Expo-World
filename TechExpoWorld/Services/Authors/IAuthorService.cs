namespace TechExpoWorld.Services.Authors
{
    using System.Threading.Tasks;

    public interface IAuthorService
    {
        Task<bool> IsAuthor(string userId);

        Task<int> AuthorId(string userId);

        Task<int> Create(
            string name,
            string phoneNumber,
            string address,
            string photoUrl,
            string userId);
    }
}
