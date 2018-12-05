namespace Actio.Common.Mongo
{
    using System.Threading.Tasks;

    public interface IDatabaseSeeder
    {
        Task SeedAsync();
    }
}