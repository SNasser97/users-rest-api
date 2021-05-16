namespace users_logic.User.Logic.Command
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using users_data.Entities;
    using users_data.Repositories;
    using users_logic.Exceptions.Command;
    using users_logic.Exceptions.User;
    using users_logic.Extensions;
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
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            IEnumerable<BaseUserRecordWithId> records = await this.userReadRepository.GetAsync();

            if (await this.userLogicFacade.DoesUserEmailAlreadyExistAsync(records, request.Email))
            {
                throw new CommandRequestException("Email already exists");
            }

            int age = await this.userLogicFacade.GetCalculatedUsersAgeAsync(request.DateOfBirth);

            if (!await this.userLogicFacade.IsAgeValidAsync(age))
            {
                throw new CommandRequestException("Ages 18 to 110 can only make a user!");
            }

            CreateUserRecord newUserRecord = request.ToRecord(age);

            Guid recordCreatedId = await this.userWriteRepository.CreateAsync(newUserRecord);

            if (recordCreatedId == Guid.Empty)
            {
                throw new CommandResponseException("No response Id was created");
            }

            return new CreateUserCommandResponse { Id = recordCreatedId };
        }

        public async Task<BaseUserCommandResponse> UpdateUserAsync(BaseUserCommandRequestWithId request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Id == Guid.Empty)
            {
                throw new CommandRequestException("Request Id cannot be empty");
            }

            UserRecord foundUserRecord = (UserRecord)await this.userReadRepository.GetAsync(request.Id);

            if (foundUserRecord == null)
            {
                throw new UserNotFoundException("User not found");
            }

            IEnumerable<BaseUserRecordWithId> userRecords = await this.userReadRepository.GetAsync();

            if (foundUserRecord.Email != request.Email && await this.userLogicFacade.DoesUserEmailAlreadyExistAsync(userRecords, request.Email))
            {
                throw new CommandRequestException("Email to update already exists");
            }

            int age = await this.userLogicFacade.GetCalculatedUsersAgeAsync(request.DateOfBirth);

            if (!await this.userLogicFacade.IsAgeValidAsync(age))
            {
                throw new CommandRequestException("Invalid date of birth");
            }

            UpdateUserRecord updatedUserRecord = request.ToRecord(age);

            Guid updatedResponseId = await this.userWriteRepository.UpdateAsync(updatedUserRecord);

            if (updatedResponseId == Guid.Empty)
            {
                throw new CommandResponseException("Response Id was empty");
            }

            return new UpdateUserCommandResponse { Id = updatedResponseId };
        }

        public async Task DeleteUserAsync(DeleteUserCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Id == Guid.Empty)
            {
                throw new CommandRequestException("Request Id cannot be empty");
            }

            UserRecord recordExists = (UserRecord)await this.userReadRepository.GetAsync(request.Id);

            if (recordExists == null)
            {
                throw new UserNotFoundException("User not found");
            }

            await this.userWriteRepository.DeleteAsync(recordExists.Id);
        }
    }
}