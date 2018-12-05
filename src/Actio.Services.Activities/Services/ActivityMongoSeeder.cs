namespace Actio.Services.Activities.Services
{
    using Actio.Common.Mongo;
    using Actio.Services.Activities.Domain.Models;
    using Actio.Services.Activities.Domain.Repositories;
    using MongoDB.Driver;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ActivityMongoSeeder : MongoSeeder
    {
        private readonly ICategoryRepository categoryRepository;

        public ActivityMongoSeeder(IMongoDatabase database
            , ICategoryRepository categoryRepository
            ) : base(database)
        {
            this.categoryRepository = categoryRepository;
        }

        protected override async Task CustomSeedAsync()
        {
            IList<string> categories = new List<string>()
            {
                "work",
                "sport",
                "hobby"
            };
            await Task.WhenAll(categories.Select(catg =>
                categoryRepository.AddAsync(new Category(catg))));
        }
    }
}