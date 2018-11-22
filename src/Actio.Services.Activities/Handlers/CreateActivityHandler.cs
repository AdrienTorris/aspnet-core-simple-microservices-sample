namespace Actio.Services.Activities.Handlers
{
    using Actio.Common.Commands;
    using RawRabbit;
    using Actio.Common.RabbitMq;
    using System.Threading.Tasks;
    using System;
    using Actio.Common.Events;

    public class CreateActivityHandler : ICommandHandler<CreateActivity>
    {
        private readonly IBusClient _busClient;

        public CreateActivityHandler(IBusClient busClient)
        {
            this._busClient=busClient;
        }

        public async Task HandleAsync(CreateActivity command){

            Console.WriteLine($"Creating activity: {command.Name}");

            await this._busClient.PublishAsync(new ActivityCreated(command.Id,
            command.UserId, command.Category, command.Name));
        }
    }
}