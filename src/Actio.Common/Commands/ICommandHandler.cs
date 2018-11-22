namespace Actio.Common.Commands
{
    using System.Threading.Tasks;

    public interface ICommandHandler<in T> where T : ICommand
    {
        Task HandleAsync(T command);
    }
}