namespace Actio.Services.Activities.Domain.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Actio.Services.Activities.Domain.Models;

    public interface IActivityRepository
    {
        Task<Activity> GetAsync(string name);
        Task AddAsync(Activity activity);
    }
}