namespace Actio.Common.Events
{
    using System;

    public class CreateActivityRejected : IRejectedEvent
    {
        public Guid Id {get;}
        public string Reason { get; }
        public string Code { get; }

        protected CreateActivityRejected() // For serialization
        {
            
        }

        public CreateActivityRejected(Guid id, string code,string reason)
        {
            this.Id=id;
            this.Code=code;
            this.Reason=reason;
        }
    }
}