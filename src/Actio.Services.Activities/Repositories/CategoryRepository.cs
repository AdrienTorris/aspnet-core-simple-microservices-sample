namespace Actio.Services.Activities.Repositories
{
    using Actio.Services.Activities.Domain.Models;
    using Actio.Services.Activities.Domain.Repositories;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public sealed class CategoryRepository : ICategoryRepository
    {
        private readonly IMongoDatabase database;

        public CategoryRepository(IMongoDatabase database)
        {
            this.database = database;
        }

        private IMongoCollection<Category> Collection
            => this.database.GetCollection<Category>("Categories");

        public async Task<Category> GetAsync(string name)
            => await Collection
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.Name == name.ToLowerInvariant());

        public async Task<IEnumerable<Category>> BrowseAsync()
            => await this.Collection
            .AsQueryable()
            .ToListAsync();

        public async Task AddAsync(Category category)
            => await Collection.InsertOneAsync(category);
    }
}