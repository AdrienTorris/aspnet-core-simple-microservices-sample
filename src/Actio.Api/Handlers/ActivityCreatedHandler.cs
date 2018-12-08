namespace Actio.Api.Handlers
{
    using System;
    using System.Threading.Tasks;
    using Actio.Api.Models;
    using Actio.Api.Repositories;
    using Actio.Common.Events;

    public class ActivityCreatedHandler : IEventHandler<ActivityCreated>
    {
        private readonly IActivityRepository activityRepository;

        public ActivityCreatedHandler(IActivityRepository activityRepository)
        {
            this.activityRepository = activityRepository;
        }

        public async Task HandleAsync(ActivityCreated @event)
        {
            // It's a very basic microservices sample. 
            // In a more realistic scenario, you should not duplicate your data but do some http calls to the good service instead.
            await this.activityRepository.AddAsync(new Activity{
                Id=@event.Id,
                UserId = @event.UserId,
                Name=@event.Name,
                Description=@event.Description,
                CreatedAt=@event.CreatedAt
            });
            
            Console.WriteLine($"Activity created: {@event.Name}");
        }
    }
}