namespace Actio.Api.Handlers
{
    using System;
    using System.Threading.Tasks;
    using Actio.Common.Events;

    public class UserAuthenticatedHandler : IEventHandler<UserAuthenticated>
    {
        public async Task HandleAsync(UserAuthenticated @event)
        {
            await Task.CompletedTask;
            
            Console.WriteLine($"User authenticated: {@event.Email}");
        }
    }
}