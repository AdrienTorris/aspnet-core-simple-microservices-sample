namespace Actio.Common.Services
{
    using Actio.Common.Commands;
    using Actio.Common.Events;
    using Actio.Common.RabbitMq;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using RawRabbit;
    using System;

    public class ServiceHost : IServiceHost
    {
        private readonly IWebHost _webHost;

        public ServiceHost(IWebHost webHost)
        {
            this._webHost=webHost;
        }

        public void Run() => this._webHost.Run();

        public static HostBuilder Create<TStartup>(string[] args) where TStartup : class
        {
            Console.Title = typeof(TStartup).Namespace;

            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            var webHostBuilder = WebHost.CreateDefaultBuilder()
                .UseConfiguration(config)
                .UseStartup<TStartup>();

            return new HostBuilder(webHostBuilder.Build());
        }
    }

    public abstract class BuilderBase
    {
        public abstract ServiceHost Build();
    }

    public class HostBuilder:BuilderBase
    {
        private readonly IWebHost _webHost;
        private IBusClient _bus;

        public HostBuilder(IWebHost webHost)
        {
            this._webHost=webHost;
        }

        public BusBuilder UseRabbitMq(){
            this._bus = (IBusClient)this._webHost.Services.GetService(typeof(IBusClient));
            return new BusBuilder(_webHost, _bus);
        }

        public override ServiceHost Build()
        {
           return new ServiceHost(_webHost);
        }
    }

    public class BusBuilder : BuilderBase
    {
        private readonly IWebHost _webHost;
        private IBusClient _bus;

        public BusBuilder(IWebHost webHost,IBusClient bus)
        {
            this._webHost=webHost;
            this._bus=bus;
        }

        public BusBuilder SubscribeToCommand<TCommand>() where TCommand: ICommand
        {
            var handler = (ICommandHandler<TCommand>)_webHost.Services
            .GetService(typeof(ICommandHandler<TCommand>));
            
            _bus.WithCommandHandlerAsync(handler);

            return this;
        }

        public BusBuilder SubscribeToEvent<TEvent>() where TEvent: IEvent
        {
            var handler = (IEventHandler<TEvent>)_webHost.Services
                .GetService(typeof(IEventHandler<TEvent>));

            _bus.WithEventHandlerAsync(handler);

            return this;
        }

        public override ServiceHost Build()
        {
           return new ServiceHost(_webHost);
        }
    }
}