namespace users_logic.User.Logic.Command
{
    using System.Threading.Tasks;
    using users_logic.User.Logic.Command.Models.Request;
    using users_logic.User.Logic.Command.Models.Response;

    public class UserCommand : IUserCommand<CreateUserCommandRequest, UpdateUserCommandRequest, BaseUserCommandResponse>
    {
        public Task<BaseUserCommandResponse> CreateUserAsync(CreateUserCommandRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task<BaseUserCommandResponse> UpdateUserAsync(UpdateUserCommandRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}