namespace Actio.Services.Identity.Domain.Repositories
{
    using System;
    using System.Threading.Tasks;
    using Actio.Common.Exceptions;
    using Actio.Services.Identity.Domain.Models;

    public interface IUserRepository
    {
        Task<User> GetAsync(Guid id);
        Task<User> GetAsync(string email);
        Task AddAsync(User user);
    }
}