namespace Actio.Common.Events
{
    using System; 

    /// 
    public interface IAuthenticatedEvent : IEvent
    {
        Guid UserId {get;}
    }
}