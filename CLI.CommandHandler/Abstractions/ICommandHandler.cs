using System.Threading.Tasks;

namespace CLI.CommandHandler.Abstractions
{
    public interface ICommandHandler<T>  where T : ICommand
    {
        Task RunAsync(T command);
    }
}