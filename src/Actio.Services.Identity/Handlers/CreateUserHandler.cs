namespace Actio.Services.Identity.Handlers
{
    using Actio.Common.Commands;
    using RawRabbit;
    using Actio.Common.RabbitMq;
    using System.Threading.Tasks;
    using System;
    using Actio.Common.Events;
    using Actio.Services.Identity.Domain.Services;
    using Actio.Common.Exceptions;
    using Microsoft.Extensions.Logging;
    using Actio.Services.Identity.Domain.Models;

    public class CreateUserHandler : ICommandHandler<CreateUser>
    {
        private readonly IBusClient _busClient;
        private ILogger logger;

        public CreateUserHandler(IBusClient busClient,
            ILogger<CreateUserHandler> logger)
        {
            this._busClient=busClient;
            this.logger = logger;
        }

        public async Task HandleAsync(CreateUser command){

            Console.WriteLine($"Creating user: {command.Email}");
            this.logger.LogInformation($"Creating user: {command.Email}");

            try
            {
                var user = new User(command.Email, command.Name);
                //TODO

                // await this.activityService.AddAsync(command.Id, command.UserId, command.Category, command.Name,
                //     command.Description, command.CreatedAt);

                // await this._busClient.PublishAsync(new ActivityCreated(command.Id,
                //     command.UserId, command.Category, command.Name));

                    return;
            }
            catch(ActioException ex)
            {
                await this._busClient.PublishAsync(
                    new CreateUserRejected(command.Email, ex.Code, ex.Message)); 

                this.logger.LogError(ex.Message);
            }
            catch(Exception ex)
            {
                await this._busClient.PublishAsync(
                    new CreateUserRejected(command.Email, "error", ex.Message)); 

                this.logger.LogError(ex.Message);
            }
        }
    }
}