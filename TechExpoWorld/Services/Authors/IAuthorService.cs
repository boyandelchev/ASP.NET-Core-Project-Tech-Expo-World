namespace TechExpoWorld.Services.Authors
{
    public interface IAuthorService
    {
        bool IsAuthor(string userId);

        int AuthorId(string userId);

        int Create(
            string name,
            string phoneNumber,
            string address,
            string photoUrl,
            string userId);
    }
}
