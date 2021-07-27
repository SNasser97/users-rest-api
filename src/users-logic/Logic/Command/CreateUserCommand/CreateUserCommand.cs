using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using users_data.Entities;
using users_data.Repositories;
using users_logic.Exceptions.Command;
using users_logic.Exceptions.Validation;
using users_logic.Extensions;
using users_logic.Facades;
using users_logic.Logic.Command.Common;

namespace users_logic.Logic.Command.CreateUserCommand
{
    public class CreateUserCommand : BaseCommandRequest<CreateUserCommandRequest, CreateUserCommandResponse, User>, ICreateUserCommand
    {
        private readonly IUserLogicFacade userLogicFacade;

        public CreateUserCommand(
            IWriteRepository<User> writeRepository,
            IReadRepository<User> readRepository,
            IUserLogicFacade userLogicFacade) : base(writeRepository, readRepository)
        {
            this.userLogicFacade = userLogicFacade;
        }

        protected override async Task<CreateUserCommandResponse> OnExecuteAsync(CreateUserCommandRequest request)
        {
            UserLogic.ThrowException<ArgumentNullException>(() => request == null, nameof(request));

            IEnumerable<User> records = await this.readRepository.GetAsync();

            await UserLogic.ThrowExceptionAsync<EmailExistsException>(async ()
                => await this.userLogicFacade.DoesUserEmailAlreadyExistAsync(records, request.Email));

            int age = await this.userLogicFacade.GetCalculatedUsersAgeAsync(request.DateOfBirth);

            await UserLogic.ThrowExceptionAsync<InvalidAgeException>(async () => !await this.userLogicFacade.IsAgeValidAsync(age));

            Guid recordCreatedId = await this.writeRepository.CreateAsync(request.ToRecord(age));

            UserLogic.ThrowException<CommandResponseException>(() => recordCreatedId == Guid.Empty);
            return new CreateUserCommandResponse { Id = recordCreatedId };
        }
    }
}