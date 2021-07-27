using System.Threading.Tasks;

namespace users_logic.Logic.Command
{
    public interface ICommand<TCommandRequest, TCommandResponse>
    {
        Task<TCommandResponse> ExecuteAsync(TCommandRequest request);
    }
}