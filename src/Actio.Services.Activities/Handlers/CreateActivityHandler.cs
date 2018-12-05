namespace Actio.Services.Activities.Handlers
{
    using Actio.Common.Commands;
    using RawRabbit;
    using Actio.Common.RabbitMq;
    using System.Threading.Tasks;
    using System;
    using Actio.Common.Events;
    using Actio.Services.Activities.Services;
    using Actio.Common.Exceptions;
    using Microsoft.Extensions.Logging;

    public class CreateActivityHandler : ICommandHandler<CreateActivity>
    {
        private readonly IBusClient _busClient;
        private readonly IActivityService activityService;
        private ILogger logger;

        public CreateActivityHandler(IBusClient busClient, IActivityService activityService,
            ILogger<CreateActivityHandler> logger)
        {
            this._busClient=busClient;
            this.activityService=activityService;
            this.logger = logger;
        }

        public async Task HandleAsync(CreateActivity command){

            Console.WriteLine($"Creating activity: {command.Category} {command.Name}");
            this.logger.LogInformation($"Creating activity: {command.Category} {command.Name}");

            try
            {
                await this.activityService.AddAsync(command.Id, command.UserId, command.Category, command.Name,
                    command.Description, command.CreatedAt);

                await this._busClient.PublishAsync(new ActivityCreated(command.Id,
                    command.UserId, command.Category, command.Name));

                    return;
            }
            catch(ActioException ex)
            {
                await this._busClient.PublishAsync(
                    new CreateActivityRejected(command.Id, ex.Code, ex.Message)); 

                this.logger.LogError(ex.Message);
            }
            catch(Exception ex)
            {
                await this._busClient.PublishAsync(
                    new CreateActivityRejected(command.Id, "error", ex.Message)); 

                this.logger.LogError(ex.Message);
            }
        }
    }
}