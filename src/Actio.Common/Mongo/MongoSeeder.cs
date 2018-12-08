namespace Actio.Common.Mongo
{
    using MongoDB.Driver;
    using System.Linq;
    using System.Threading.Tasks;

    public class MongoSeeder : IDatabaseSeeder
    {
        protected readonly IMongoDatabase Database;

        public MongoSeeder(IMongoDatabase database)
        {
            this.Database = database;
        }

      public async  Task SeedAsync()
      {
          var collectionCursor = await this.Database.ListCollectionsAsync();
          var collections = await collectionCursor.ToListAsync();

          if(collections.Any())
          {
              return;
          }

          await CustomSeedAsync();
      }

      protected virtual async Task CustomSeedAsync()
      {
          await Task.CompletedTask;
      }
    }
}