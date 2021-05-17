namespace users_test.Users_logic_tests.Command
{
    using Xunit;
    using System;
    using users_logic.User.Logic.Command;
    using Moq;
    using users_data.Repositories;
    using users_data.Entities;
    using users_logic.User.Facades;
    using System.Threading.Tasks;
    using users_logic.User.Logic.Command.Models.Request;
    using users_logic.Exceptions.Command;
    using users_logic.Exceptions.User;

    public class UserCommandDeleteUserAsyncTests
    {
        private Mock<IWriteRepository<BaseUserRecord, BaseUserRecordWithId>> mockUserWriteRepository;
        private Mock<IReadRepository<BaseUserRecordWithId>> mockUserReadRepository;
        private Mock<IUserLogicFacade> mockUserLogicFacade;
        private UserCommand userCommand;

        public UserCommandDeleteUserAsyncTests()
        {
            this.mockUserWriteRepository = new Mock<IWriteRepository<BaseUserRecord, BaseUserRecordWithId>>();
            this.mockUserReadRepository = new Mock<IReadRepository<BaseUserRecordWithId>>();
            this.mockUserLogicFacade = new Mock<IUserLogicFacade>();
            this.userCommand = new UserCommand(this.mockUserWriteRepository.Object, this.mockUserReadRepository.Object, this.mockUserLogicFacade.Object);
        }

        [Fact]
        public async Task UserCommand_DeleteUserAsync_TakesNullDeleteUserCommandRequest_ExpectsArgumentNullException()
        {
            //Given
            //When
            //Then
            await Exceptions<ArgumentNullException>.HandleAsync(async () =>
                await this.userCommand.DeleteUserAsync(null),
                (ex) => Assert.Equal("request", ex.ParamName)
            );
        }

        [Fact]
        public async Task UserCommand_DeleteUserAsync_TakesDeleteUserCommandRequestWithEmptyGuid_ExpectsCommandRequestException()
        {
            //Given
            //When
            //Then
            await Exceptions<CommandRequestException>.HandleAsync(async () =>
                await this.userCommand.DeleteUserAsync(new DeleteUserCommandRequest()),
                (ex) => Assert.Equal("Request Id was empty", ex.Message)
            );
        }

        [Fact]
        public async Task UserCommand_DeleteUserAsync_TakesDeleteUserCommandRequest_ExpectsToDeleteUserRecord()
        {
            //Given
            Guid requestId = Guid.NewGuid();
            var deleteUserCommandRequest = new DeleteUserCommandRequest { Id = requestId };
            var userRecord = new UserRecord
            {
                Id = requestId,
                FirstName = "Bob",
                LastName = "Doe",
                Email = "b.doe@email.com",
                DateOfBirth = new DateTime(1992, 12, 01),
                Age = 28
            };

            this.mockUserReadRepository.Setup(s => s.GetAsync(It.IsAny<Guid>())).ReturnsAsync(userRecord);
            this.mockUserWriteRepository.Setup(s => s.DeleteAsync(It.IsAny<Guid>()));

            //When
            await this.userCommand.DeleteUserAsync(deleteUserCommandRequest);

            //Then
            this.mockUserReadRepository.Verify(s => s.GetAsync(It.IsAny<Guid>()), Times.Once);
            this.mockUserWriteRepository.Verify(s => s.DeleteAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task UserCommand_DeleteUserAsync_TakesDeleteUserCommandRequest_ThrowsUserNotFoundException()
        {
            //Given
            var deleteUserCommandRequest = new DeleteUserCommandRequest { Id = Guid.NewGuid() };

            this.mockUserReadRepository.Setup(s => s.GetAsync(It.IsAny<Guid>())).ReturnsAsync(null as UserRecord);
            this.mockUserWriteRepository.Setup(s => s.DeleteAsync(It.IsAny<Guid>()));

            //When
            await Exceptions<UserNotFoundException>.HandleAsync(async () =>
                await this.userCommand.DeleteUserAsync(deleteUserCommandRequest),
                (ex) => Assert.Equal("User not found", ex.Message)
            );

            //Then
            this.mockUserReadRepository.Verify(s => s.GetAsync(It.IsAny<Guid>()), Times.Once);
            this.mockUserWriteRepository.Verify(s => s.DeleteAsync(It.IsAny<Guid>()), Times.Never);
        }
    }
}