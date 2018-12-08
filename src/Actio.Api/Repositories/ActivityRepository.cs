using System.Threading.Tasks;
using Actio.Api.Models;
using System;
using System.Collections.Generic;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;

namespace Actio.Api.Repositories
{
    public sealed class ActivityRepository : IActivityRepository
    {
        private readonly IMongoDatabase database;

        public ActivityRepository(IMongoDatabase database)
        {
            this.database = database;
        }

        private IMongoCollection<Activity> Collection
            => this.database.GetCollection<Activity>("Activities");

        public async  Task<Activity> GetAsync(Guid id)
            => await this.Collection
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.Id == id);

        public async Task AddAsync(Activity activity)
            => await this.Collection.InsertOneAsync(activity);

        public async  Task<IEnumerable<Activity>> BrowseAsync(Guid userId)
            => await this.Collection
            .AsQueryable()
            .Where(x=>x.UserId==userId)
            .ToListAsync();
    }
}