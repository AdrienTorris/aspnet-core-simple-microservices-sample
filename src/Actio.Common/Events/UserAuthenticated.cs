namespace Actio.Common.Events
{
    using System;

    public class UserAuthenticated : IEvent
    {
        public string Email {get;}

        protected UserAuthenticated()
        {
            
        }

        public UserAuthenticated(string email)
        {
            this.Email=email;
        }
    }
}