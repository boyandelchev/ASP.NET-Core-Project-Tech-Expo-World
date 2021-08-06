namespace TechExpoWorld.Services.Authors
{
    using System.Linq;
    using TechExpoWorld.Data;
    using TechExpoWorld.Data.Models;

    public class AuthorService : IAuthorService
    {
        private readonly TechExpoDbContext data;

        public AuthorService(TechExpoDbContext data)
            => this.data = data;

        public bool IsAuthor(string userId)
            => this.data
                .Authors
                .Any(a => a.UserId == userId);

        public int AuthorId(string userId)
            => this.data
                .Authors
                .Where(a => a.UserId == userId)
                .Select(a => a.Id)
                .FirstOrDefault();

        public int Create(
            string name,
            string phoneNumber,
            string address,
            string photoUrl,
            string userId)
        {
            var author = new Author
            {
                Name = name,
                PhoneNumber = phoneNumber,
                Address = address,
                PhotoUrl = photoUrl,
                UserId = userId
            };

            this.data.Authors.Add(author);
            this.data.SaveChanges();

            return author.Id;
        }
    }
}
