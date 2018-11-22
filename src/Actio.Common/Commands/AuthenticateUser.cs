namespace Actio.Common.Commands
{
    /// 
    public class AutenticateUser : ICommand
    {
        public string Email {get;set;}
        public string Password {get;set;}
    }
}