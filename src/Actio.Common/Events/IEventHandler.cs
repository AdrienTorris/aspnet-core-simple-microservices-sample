namespace Actio.Common.Events
{ 
    using System.Threading.Tasks;

    public interface IEventHandler<in T> where T:IEvent
    {
        Task HandleAsync(T @event); // We have to user an arobase in the parameter name because 'event' is a reserved word by C#
    }
}