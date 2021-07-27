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

    public class UserCommand : IUserCommand<BaseUserCommandResponseModel>
    {
        private readonly IWriteRepository<User> userWriteRepository;
        private readonly IReadRepository<User> userReadRepository;
        private readonly IUserLogicFacade userLogicFacade;

        public UserCommand(
            IWriteRepository<User> userWriteRepository,
            IReadRepository<User> userReadRepository,
            IUserLogicFacade userLogicFacade)
        {
            this.userWriteRepository = userWriteRepository ?? throw new System.ArgumentNullException(nameof(userWriteRepository));
            this.userReadRepository = userReadRepository ?? throw new System.ArgumentNullException(nameof(userReadRepository));
            this.userLogicFacade = userLogicFacade ?? throw new System.ArgumentNullException(nameof(userLogicFacade));
        }

        public async Task<BaseUserCommandResponseModel> CreateUserAsync(BaseUserCommandRequestModel request)
        {
            UserLogic.ThrowException<ArgumentNullException>(() => request == null, nameof(request));

            IEnumerable<User> records = await this.userReadRepository.GetAsync();

            await UserLogic.ThrowExceptionAsync<EmailExistsException>(async ()
                => await this.userLogicFacade.DoesUserEmailAlreadyExistAsync(records, request.Email));

            int age = await this.userLogicFacade.GetCalculatedUsersAgeAsync(request.DateOfBirth);

            await UserLogic.ThrowExceptionAsync<InvalidAgeException>(async () => !await this.userLogicFacade.IsAgeValidAsync(age));

            Guid recordCreatedId = await this.userWriteRepository.CreateAsync(request.ToRecord(age));

            UserLogic.ThrowException<CommandResponseException>(() => recordCreatedId == Guid.Empty);
            return new CreateUserCommandResponseModel { Id = recordCreatedId };
        }

        public async Task<BaseUserCommandResponseModel> UpdateUserAsync(BaseUserCommandRequestWithIdModel request)
        {
            UserLogic.ThrowException<ArgumentNullException>(() => request == null, nameof(request));
            UserLogic.ThrowException<CommandRequestException>(() => request.Id == Guid.Empty);

            User foundUserRecord = await this.userReadRepository.GetAsync(request.Id);
            UserLogic.ThrowException<UserNotFoundException>(() => foundUserRecord == null);

            IEnumerable<User> userRecords = await this.userReadRepository.GetAsync();

            await UserLogic.ThrowExceptionAsync<EmailExistsException>(async ()
                => foundUserRecord.Email != request.Email && await this.userLogicFacade.DoesUserEmailAlreadyExistAsync(userRecords, request.Email));

            int age = await this.userLogicFacade.GetCalculatedUsersAgeAsync(request.DateOfBirth);

            await UserLogic.ThrowExceptionAsync<InvalidDateOfBirthException>(async () => !await this.userLogicFacade.IsAgeValidAsync(age));

            Guid updatedResponseId = await this.userWriteRepository.UpdateAsync(request.ToRecord(age));

            UserLogic.ThrowException<CommandResponseException>(() => updatedResponseId == Guid.Empty);
            return new UpdateUserCommandResponseModel { Id = updatedResponseId };
        }

        public async Task DeleteUserAsync(DeleteUserCommandRequestModel request)
        {
            UserLogic.ThrowException<ArgumentNullException>(() => request == null, nameof(request));
            UserLogic.ThrowException<CommandRequestException>(() => request.Id == Guid.Empty);

            User recordExists = await this.userReadRepository.GetAsync(request.Id);

            UserLogic.ThrowException<UserNotFoundException>(() => recordExists == null);
            await this.userWriteRepository.DeleteAsync(recordExists.Id);
        }
    }
}