namespace Actio.Api.Handlers
{
    using System;
    using System.Threading.Tasks;
    using Actio.Common.Events;

    public class ActivityCreatedHandler : IEventHandler<ActivityCreated>
    {
        public async Task HandleAsync(ActivityCreated @event)
        {
            await Task.CompletedTask;
            
            Console.WriteLine($"Activity created: {@event.Name}");
        }
    }
}