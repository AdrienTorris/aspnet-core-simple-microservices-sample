namespace Actio.Common.Mongo
{
    using System.Threading.Tasks;

    public interface IDatabaseInitializer
    {
        // Task InitializeAsync();
         Task InitializeAsync(IDatabaseSeeder seeder);
    }
}