﻿namespace TechExpoWorld.Services.Authors
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using TechExpoWorld.Data;
    using TechExpoWorld.Data.Models;

    public class AuthorsService : IAuthorsService
    {
        private readonly TechExpoDbContext data;

        public AuthorsService(TechExpoDbContext data)
            => this.data = data;

        public async Task<bool> IsAuthorAsync(string userId)
            => await this.data
                .Authors
                .AnyAsync(a => a.UserId == userId);

        public async Task<string> AuthorIdAsync(string userId)
            => await this.data
                .Authors
                .Where(a => a.UserId == userId)
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
                UserId = userId
            };

            await this.data.Authors.AddAsync(author);
            await this.data.SaveChangesAsync();

            return author.Id;
        }
    }
}