namespace Actio.Services.Activities.Services
{
    using System;
    using System.Threading.Tasks;

    public interface IActivityService
    {
        Task AddAsync(Guid id, Guid userId, string category,
            string name, string description, DateTime createdAt);
    }
}