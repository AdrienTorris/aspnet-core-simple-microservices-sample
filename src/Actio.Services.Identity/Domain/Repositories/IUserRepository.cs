namespace Actio.Services.Identity.Domain.Repositories
{
    using Actio.Services.Identity.Domain.Models;
    using System;
    using System.Threading.Tasks;

    public interface IUserRepository
    {
        Task<User> GetAsync(Guid id);
        Task<User> GetAsync(string email);
        Task AddAsync(User user);
    }
}