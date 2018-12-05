using Actio.Services.Identity.Domain.Models;
using Actio.Services.Identity.Domain.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Threading.Tasks;

namespace Actio.Services.Identity.Repositories
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly IMongoDatabase database;

        public UserRepository(IMongoDatabase database)
        {
            this.database=database;
        }

        private IMongoCollection<User> Collection
            => this.database.GetCollection<User>("Users");

        public async Task<User> GetAsync(Guid id)
            => await this.Collection
            .AsQueryable()
            .FirstOrDefaultAsync(x=>x.Id==id);

        public async Task<User> GetAsync(string email)
            => await this.Collection
            .AsQueryable()
            .FirstOrDefaultAsync(x=>x.Email==email.ToLowerInvariant());

        public async Task AddAsync(User user)
            => await this.Collection
            .InsertOneAsync(user);
    }
}