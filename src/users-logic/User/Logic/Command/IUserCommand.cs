namespace users_logic.User.Logic.Command
{
    using System.Threading.Tasks;
    using users_logic.User.Logic.Command.Models.Request;
    using users_logic.User.Logic.Command.Models.Request.Common;

    public interface IUserCommand<TResponse>
    {
        Task<TResponse> CreateUserAsync(BaseUserCommandRequestModel request);

        Task<TResponse> UpdateUserAsync(BaseUserCommandRequestWithIdModel request);

        Task DeleteUserAsync(DeleteUserCommandRequestModel request);
    }
}