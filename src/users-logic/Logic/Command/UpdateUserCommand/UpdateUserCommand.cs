namespace users_logic.Logic.Command.UpdateUserCommand
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using users_data.Entities;
    using users_data.Repositories;
    using users_logic.Exceptions.Command;
    using users_logic.Exceptions.UserExceptions;
    using users_logic.Exceptions.Validation;
    using users_logic.Extensions;
    using users_logic.Facades;
    using users_logic.Logic.Command.Common;

    public class UpdateUserCommand : BaseCommandRequest<UpdateUserCommandRequest, UpdateUserCommandResponse, User>, IUpdateUserCommand
    {
        private readonly IUserLogicFacade userLogicFacade;

        public UpdateUserCommand(
            IWriteRepository<User> writeRepository,
            IReadRepository<User> readRepository,
            IUserLogicFacade userLogicFacade) : base(writeRepository, readRepository)
        {
            this.userLogicFacade = userLogicFacade;
        }

        protected override async Task<UpdateUserCommandResponse> OnExecuteAsync(UpdateUserCommandRequest request)
        {
            UserLogic.ThrowException<ArgumentNullException>(() => request == null, nameof(request));
            UserLogic.ThrowException<CommandRequestException>(() => request.Id == Guid.Empty);

            User foundUserRecord = await this.readRepository.GetAsync(request.Id);
            UserLogic.ThrowException<UserNotFoundException>(() => foundUserRecord == null);

            IEnumerable<User> userRecords = await this.readRepository.GetAsync();

            await UserLogic.ThrowExceptionAsync<EmailExistsException>(async ()
                => foundUserRecord.Email != request.Email && await this.userLogicFacade.DoesUserEmailAlreadyExistAsync(userRecords, request.Email));

            int age = await this.userLogicFacade.GetCalculatedUsersAgeAsync(request.DateOfBirth);

            await UserLogic.ThrowExceptionAsync<InvalidDateOfBirthException>(async () => !await this.userLogicFacade.IsAgeValidAsync(age));

            Guid updatedResponseId = await this.writeRepository.UpdateAsync(request.ToRecord(age));

            UserLogic.ThrowException<CommandResponseException>(() => updatedResponseId == Guid.Empty);
            return new UpdateUserCommandResponse { Id = updatedResponseId };
        }
    }
}