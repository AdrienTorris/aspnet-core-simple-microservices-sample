namespace Actio.Services.Activities.Domain.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Actio.Services.Activities.Domain.Models;

    public interface ICategoryRepository
    {
        Task<Category> GetAsync(string name);
        Task<IEnumerable<Category>> BrowseAsync();
        Task AddAsync(Category category);
    }
}