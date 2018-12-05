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
    using Actio.Services.Identity.Services;

    public class CreateUserHandler : ICommandHandler<CreateUser>
    {
        private readonly IBusClient _busClient;
        private ILogger logger;
        private readonly IUserService userService;

        public CreateUserHandler(IBusClient busClient,
            ILogger<CreateUserHandler> logger,
            IUserService userService)
        {
            this._busClient=busClient;
            this.logger = logger;
            this.userService=userService;
        }

        public async Task HandleAsync(CreateUser command)
        {
            Console.WriteLine($"Creating user: {command.Email}");
            this.logger.LogInformation($"Creating user: {command.Email}");

            try
            {
                 await this.userService.RegisterAsync(command.Email, command.Password, command.Name);

                 await this._busClient.PublishAsync(new UserCreated(command.Email,command.Name));

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