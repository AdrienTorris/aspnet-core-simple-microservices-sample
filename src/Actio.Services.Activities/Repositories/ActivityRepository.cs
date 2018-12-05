namespace Actio.Services.Activities.Repositories
{
    using Actio.Services.Activities.Domain.Models;
    using Actio.Services.Activities.Domain.Repositories;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;
    using System.Threading.Tasks;

    public sealed class ActivityRepository : IActivityRepository
    {
        private readonly IMongoDatabase database;

        public ActivityRepository(IMongoDatabase database)
        {
            this.database = database;
        }

        private IMongoCollection<Activity> Collection
            => this.database.GetCollection<Activity>("Activities");

        public async Task<Activity> GetAsync(string name)
            => await this.Collection
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.Name == name.ToLowerInvariant());

        public async Task AddAsync(Activity activity)
            => await this.Collection.InsertOneAsync(activity);
    }
}