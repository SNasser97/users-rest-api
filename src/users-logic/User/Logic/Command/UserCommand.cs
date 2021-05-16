namespace users_logic.User.Logic.Command
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using users_data.Entities;
    using users_data.Repositories;
    using users_logic.Exceptions.Command;
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

        public async Task<BaseUserCommandResponse> CreateUserAsync(CreateUserCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            IEnumerable<UserRecord> records = await this.userReadRepository.GetAsync();

            if (await this.userLogicFacade.DoesUserEmailAlreadyExistAsync(records, request.Email))
            {
                throw new CommandRequestException("Email already exists");
            }

            int age = await this.userLogicFacade.GetCalculatedUsersAgeAsync(request.DateOfBirth);

            if (!await this.userLogicFacade.IsAgeValidAsync(age))
            {
                throw new CommandRequestException("Ages 18 to 110 can only make a user!");
            }

            // TODO: Extension method
            // TODO: Entities to create data : CreateUserRecord, UpdateUserRecord
            UserRecord newRecord = new UserRecord
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                DateOfBirth = request.DateOfBirth,
            };

            Guid recordCreatedId = await this.userWriteRepository.CreateAsync(newRecord);

            if (recordCreatedId == Guid.Empty)
            {
                throw new CommandResponseException("No response Id was returned");
            }

            return new CreateUserCommandResponse { Id = recordCreatedId };
        }

        public async Task<BaseUserCommandResponse> UpdateUserAsync(UpdateUserCommandRequest request)
        {
            throw new System.NotImplementedException();
        }

        public async Task DeleteUserAsync(DeleteUserCommandRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}