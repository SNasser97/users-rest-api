namespace users_logic.User.Logic.Command
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using users_data.Entities;
    using users_data.Repositories;
    using users_logic.Exceptions.Command;
    using users_logic.Exceptions.User;
    using users_logic.Exceptions.Validation;
    using users_logic.Extensions;
    using users_logic.User.Exceptions.Validation;
    using users_logic.User.Facades;
    using users_logic.User.Logic.Command.Models.Request;
    using users_logic.User.Logic.Command.Models.Request.Common;
    using users_logic.User.Logic.Command.Models.Response;

    public class UserCommand : IUserCommand<BaseUserCommandResponse>
    {
        private readonly IWriteRepository<BaseUserRecord, BaseUserRecordWithId> userWriteRepository;
        private readonly IReadRepository<BaseUserRecordWithId> userReadRepository;
        private readonly IUserLogicFacade userLogicFacade;

        public UserCommand(
            IWriteRepository<BaseUserRecord, BaseUserRecordWithId> userWriteRepository,
            IReadRepository<BaseUserRecordWithId> userReadRepository,
            IUserLogicFacade userLogicFacade)
        {
            this.userWriteRepository = userWriteRepository ?? throw new System.ArgumentNullException(nameof(userWriteRepository));
            this.userReadRepository = userReadRepository ?? throw new System.ArgumentNullException(nameof(userReadRepository));
            this.userLogicFacade = userLogicFacade ?? throw new System.ArgumentNullException(nameof(userLogicFacade));
        }

        public async Task<BaseUserCommandResponse> CreateUserAsync(BaseUserCommandRequest request)
        {
            ExecuteLogic.ThrowException<ArgumentNullException>(() => request == null, nameof(request));

            IEnumerable<BaseUserRecordWithId> records = await this.userReadRepository.GetAsync();

            await ExecuteLogic.ThrowExceptionAsync<EmailExistsException>(async ()
                => await this.userLogicFacade.DoesUserEmailAlreadyExistAsync(records, request.Email));

            int age = await this.userLogicFacade.GetCalculatedUsersAgeAsync(request.DateOfBirth);

            await ExecuteLogic.ThrowExceptionAsync<InvalidDateOfBirthException>(async () => !await this.userLogicFacade.IsAgeValidAsync(age));

            CreateUserRecord newUserRecord = request.ToRecord(age);

            Guid recordCreatedId = await this.userWriteRepository.CreateAsync(newUserRecord);

            ExecuteLogic.ThrowException<CommandResponseException>(() => recordCreatedId == Guid.Empty);


            return new CreateUserCommandResponse { Id = recordCreatedId };
        }

        public async Task<BaseUserCommandResponse> UpdateUserAsync(BaseUserCommandRequestWithId request)
        {
            ExecuteLogic.ThrowException<ArgumentNullException>(() => request == null, nameof(request));
            ExecuteLogic.ThrowException<CommandRequestException>(() => request.Id == Guid.Empty);

            UserRecord foundUserRecord = (UserRecord)await this.userReadRepository.GetAsync(request.Id);
            ExecuteLogic.ThrowException<UserNotFoundException>(() => foundUserRecord == null);

            IEnumerable<BaseUserRecordWithId> userRecords = await this.userReadRepository.GetAsync();

            await ExecuteLogic.ThrowExceptionAsync<EmailExistsException>(async ()
                => foundUserRecord.Email != request.Email && await this.userLogicFacade.DoesUserEmailAlreadyExistAsync(userRecords, request.Email));

            int age = await this.userLogicFacade.GetCalculatedUsersAgeAsync(request.DateOfBirth);

            await ExecuteLogic.ThrowExceptionAsync<InvalidAgeException>(async () => !await this.userLogicFacade.IsAgeValidAsync(age));

            UpdateUserRecord updatedUserRecord = request.ToRecord(age);

            Guid updatedResponseId = await this.userWriteRepository.UpdateAsync(updatedUserRecord);

            ExecuteLogic.ThrowException<CommandResponseException>(() => updatedResponseId == Guid.Empty);

            return new UpdateUserCommandResponse { Id = updatedResponseId };
        }

        public async Task DeleteUserAsync(DeleteUserCommandRequest request)
        {
            ExecuteLogic.ThrowException<ArgumentNullException>(() => request == null, nameof(request));
            ExecuteLogic.ThrowException<CommandRequestException>(() => request.Id == Guid.Empty);

            UserRecord recordExists = (UserRecord)await this.userReadRepository.GetAsync(request.Id);

            ExecuteLogic.ThrowException<UserNotFoundException>(() => recordExists == null);

            await this.userWriteRepository.DeleteAsync(recordExists.Id);
        }
    }
}