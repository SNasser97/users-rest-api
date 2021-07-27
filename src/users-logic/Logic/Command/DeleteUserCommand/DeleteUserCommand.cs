namespace users_logic.Logic.Command.DeleteUserCommand
{
    using System;
    using System.Threading.Tasks;
    using users_data.Entities;
    using users_data.Repositories;
    using users_logic.Exceptions.Command;
    using users_logic.Exceptions.UserExceptions;
    using users_logic.Extensions;
    using users_logic.Logic.Command.Common;

    public class DeleteUserCommand : BaseCommandRequest<DeleteUserCommandRequest, DeleteUserCommandResponse, User>, IDeleteUserCommand
    {

        public DeleteUserCommand(
            IWriteRepository<User> writeRepository,
            IReadRepository<User> readRepository) : base(writeRepository, readRepository)
        {
        }

        protected override async Task<DeleteUserCommandResponse> OnExecuteAsync(DeleteUserCommandRequest request)
        {
            UserLogic.ThrowException<ArgumentNullException>(() => request == null, nameof(request));
            UserLogic.ThrowException<CommandRequestException>(() => request.Id == Guid.Empty);

            User recordExists = await this.readRepository.GetAsync(request.Id);

            UserLogic.ThrowException<UserNotFoundException>(() => recordExists == null);
            await this.writeRepository.DeleteAsync(recordExists.Id);
            return new DeleteUserCommandResponse();
        }
    }
}