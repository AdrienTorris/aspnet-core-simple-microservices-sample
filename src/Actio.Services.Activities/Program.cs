using Actio.Common.Commands;
using Actio.Common.Services;

namespace Actio.Services.Activities
{
    public class Program
    {
        // public static void Main(string[] args)
        // {
        //     CreateWebHostBuilder(args).Build().Run();
        // }

        // public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        //     WebHost.CreateDefaultBuilder(args)
        //         .UseStartup<Startup>();

        public static void Main(string[] args)
        {
            ServiceHost.Create<Startup>(args)
            .UseRabbitMq()
            .SubscribeToCommand<CreateActivity>()
            .Build()
            .Run();
        }
    }
}
