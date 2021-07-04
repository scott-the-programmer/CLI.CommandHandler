using System.Threading.Tasks;

namespace CLI.CommandHandler
{
    public interface ICommandDispatcher
    {
        Task DispatchAsync(object command);
    }
}