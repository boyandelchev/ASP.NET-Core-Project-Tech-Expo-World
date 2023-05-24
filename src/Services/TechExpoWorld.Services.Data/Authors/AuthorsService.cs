namespace TechExpoWorld.Services.Data.Authors
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using TechExpoWorld.Data.Common.Repositories;
    using TechExpoWorld.Data.Models;

    public class AuthorsService : IAuthorsService
    {
        private readonly IDeletableEntityRepository<Author> authorsRepository;

        public AuthorsService(IDeletableEntityRepository<Author> authorsRepository)
            => this.authorsRepository = authorsRepository;

        public async Task<bool> IsAuthorAsync(string userId)
            => await this.authorsRepository
                .All()
                .AnyAsync(a => a.ApplicationUserId == userId);

        public async Task<string> AuthorIdAsync(string userId)
            => await this.authorsRepository
                .All()
                .Where(a => a.ApplicationUserId == userId)
                .Select(a => a.Id)
                .FirstOrDefaultAsync();

        public async Task<string> CreateAsync(
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
                ApplicationUserId = userId,
            };

            await this.authorsRepository.AddAsync(author);
            await this.authorsRepository.SaveChangesAsync();

            return author.Id;
        }
    }
}
