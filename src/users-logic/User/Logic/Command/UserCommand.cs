namespace users_logic.User.Logic.Command
{
    using System.Threading.Tasks;
    using users_data.Entities;
    using users_data.Repositories;
    using users_logic.User.Facades;
    using users_logic.User.Logic.Command.Models.Request;
    using users_logic.User.Logic.Command.Models.Response;

    public class UserCommand : IUserCommand<CreateUserCommandRequest, UpdateUserCommandRequest, DeleteUserCommandRequest, BaseUserCommandResponse>
    {
        private readonly IWriteRepository<UserRecord> userWriteRepository;
        private readonly IReadRepository<UserRecord> userReadRepository;
        private readonly IUserLogicFacade userLogicFacade;

        public UserCommand(
            IWriteRepository<UserRecord> userWriteRepository,
            IReadRepository<UserRecord> userReadRepository,
            IUserLogicFacade userLogicFacade)
        {
            this.userWriteRepository = userWriteRepository ?? throw new System.ArgumentNullException(nameof(userWriteRepository));
            this.userReadRepository = userReadRepository ?? throw new System.ArgumentNullException(nameof(userReadRepository));
            this.userLogicFacade = userLogicFacade ?? throw new System.ArgumentNullException(nameof(userLogicFacade));
        }

        public Task<BaseUserCommandResponse> CreateUserAsync(CreateUserCommandRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task<BaseUserCommandResponse> UpdateUserAsync(UpdateUserCommandRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteUserAsync(DeleteUserCommandRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}