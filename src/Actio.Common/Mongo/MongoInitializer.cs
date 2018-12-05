namespace Actio.Common.Mongo
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;
    using MongoDB.Driver;
    using MongoDB.Bson.Serialization.Conventions;
    using MongoDB.Bson;
    using System.Collections.Generic;

    public  class MongoInitializer : IDatabaseInitializer
    {
        private bool _initialized;
        private readonly IMongoDatabase _database;
        private readonly bool _seed;
        //private readonly IDatabaseSeeder seeder;

        public MongoInitializer(IMongoDatabase database
        //, IDatabaseSeeder seeder
        , IOptions<MongoOptions> options)
        {
            this._database=database;
            this._seed=options.Value.Seed;
           // this.seeder=seeder;
        }

        // public async Task InitializeAsync()
        // {
        //     if(this._initialized){
        //         return;
        //     }

        //     RegisterConventions();
        //     this._initialized=true;
        //     if(!this._seed){
        //         return;
        //     }


        //     await this.seeder.SeedAsync();
        // }

        public async Task InitializeAsync(IDatabaseSeeder seeder)
        {
            if(this._initialized){
                return;
            }

            RegisterConventions();
            this._initialized=true;
            if(!this._seed){
                return;
            }


            await seeder.SeedAsync();
        }

        private void RegisterConventions(){
            ConventionRegistry.Register("ActioConventions", new MongoConvention(), x=>true);
        }

        private class MongoConvention : IConventionPack
        {
            public IEnumerable<IConvention> Conventions => new List<IConvention>
            {
                new IgnoreExtraElementsConvention(true),
                new EnumRepresentationConvention(BsonType.String),
                new CamelCaseElementNameConvention()
            };
        }
    }
}