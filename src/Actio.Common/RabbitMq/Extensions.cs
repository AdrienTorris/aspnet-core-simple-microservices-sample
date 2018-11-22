namespace Actio.Common.RabbitMq
{
    using System.Threading.Tasks;
    using Actio.Common.Commands;
    using Actio.Common.Events;
    using RawRabbit;
    using RawRabbit.Pipe;
    using RawRabbit.Instantiation;
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Configuration;

    public static class Extensions
    {
        public static Task WithCommandHandlerAsync<TCommand>(this IBusClient bus, 
        ICommandHandler<TCommand> handler) where TCommand: ICommand
        => bus.SubscribeAsync<TCommand>(
            msg=> handler.HandleAsync(msg),
            ctx=>ctx.UseSubscribeConfiguration(
                cfg=>cfg.FromDeclaredQueue(
                    q=>q.WithName(GetQueueName<TCommand>())
                    )
                )
            );

// Since rawrabbit 2.0.0-rc version, UseConsumerConfiguration is no more available.
// See: https://github.com/pardahlman/RawRabbit/releases
        // public static Task WithCommandHandlerAsync<TCommand>(this IBusClient bus, 
        // ICommandHandler<TCommand> handler) where TCommand: ICommand
        // => bus.SubscribeAsync<TCommand>(
        //     msg=>handler.HandleAsync(msg),
        //     ctx=>ctx.UseConsumerConfiguration(
        //         cfg=>cfg.FromDeclaredQueue(
        //             q=>q.WithName(GetQueueName<TCommand>())
        //             )
        //         )
        //     );
            
            public static Task WithEventHandlerAsync<TEvent>(this IBusClient bus, 
        IEventHandler<TEvent> handler) where TEvent: IEvent
        => bus.SubscribeAsync<TEvent>(
            msg=>handler.HandleAsync(msg),
            ctx=>ctx.UseSubscribeConfiguration(
                cfg=>cfg.FromDeclaredQueue(
                    q=>q.WithName(GetQueueName<TEvent>())
                    )
                )
            );
            
// Since rawrabbit 2.0.0-rc version, UseConsumerConfiguration is no more available.
// See: https://github.com/pardahlman/RawRabbit/releases
        // public static Task WithEventHandlerAsync<TEvent>(this IBusClient bus, 
        // IEventHandler<TEvent> handler) where TEvent: IEvent
        // => bus.SubscribeAsync<TEvent>(
        //     msg=>handler.HandleAsync(msg),
        //     ctx=>ctx.UseConsumerConfiguration(
        //         cfg=>cfg.FromDeclaredQueue(
        //             q=>q.WithName(GetQueueName<TEvent>())
        //             )
        //         )
        //     );

        private static string GetQueueName<T>()
        => $"{Assembly.GetEntryAssembly().GetName()}/{typeof(T).Name}";

        public static void AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            var options = new RabbitMqOptions();
            var section = configuration.GetSection("rabbitmq");
            section.Bind(options);

            var client = RawRabbitFactory.CreateSingleton(new RawRabbitOptions{
                ClientConfiguration=options
            });

            services.AddSingleton<IBusClient>(_=>client);
        }
    }
}