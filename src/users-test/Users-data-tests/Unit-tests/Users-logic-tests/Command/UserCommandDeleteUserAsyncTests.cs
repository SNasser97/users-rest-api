namespace users_test.Users_logic_tests.UnitTests.Command
{
    using Xunit;
    using System;
    using Moq;
    using users_data.Repositories;
    using users_data.Entities;
    using System.Threading.Tasks;
    using users_logic.Exceptions.Command;
    using users_logic.Exceptions.UserExceptions;
    using users_logic.Logic.Command.DeleteUserCommand;
    using users_logic.Facades;
    using users_test.Helper;

    public class UserCommandDeleteUserAsyncTests
    {
        private Mock<IWriteRepository<User>> mockUserWriteRepository;
        private Mock<IReadRepository<User>> mockUserReadRepository;
        private Mock<IUserLogicFacade> mockUserLogicFacade;
        private DeleteUserCommand deleteUserCommand;

        public UserCommandDeleteUserAsyncTests()
        {
            this.mockUserWriteRepository = new Mock<IWriteRepository<User>>();
            this.mockUserReadRepository = new Mock<IReadRepository<User>>();
            this.mockUserLogicFacade = new Mock<IUserLogicFacade>();
            this.deleteUserCommand = new DeleteUserCommand(this.mockUserWriteRepository.Object, this.mockUserReadRepository.Object);
        }

        [Fact]
        public async Task UserCommand_DeleteUserAsync_TakesNullDeleteUserCommandRequest_ExpectsArgumentNullException()
        {
            //Given
            //When
            //Then
            await Exceptions<ArgumentNullException>.HandleAsync(async () =>
                await this.deleteUserCommand.ExecuteAsync(null),
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
                await this.deleteUserCommand.ExecuteAsync(new DeleteUserCommandRequest()),
                (ex) => Assert.Equal("Request Id was empty", ex.Message)
            );
        }

        [Fact]
        public async Task UserCommand_DeleteUserAsync_TakesDeleteUserCommandRequest_ExpectsToDeleteUserRecord()
        {
            //Given
            Guid requestId = Guid.NewGuid();
            var deleteUserCommandRequest = new DeleteUserCommandRequest { Id = requestId };
            var userRecord = new User
            {
                Id = requestId,
                Firstname = "Bob",
                Lastname = "Doe",
                Email = "b.doe@email.com",
                DateOfBirth = new DateTime(1992, 12, 01),
                Age = 28
            };

            this.mockUserReadRepository.Setup(s => s.GetAsync(It.IsAny<Guid>())).ReturnsAsync(userRecord);
            this.mockUserWriteRepository.Setup(s => s.DeleteAsync(It.IsAny<Guid>()));

            //When
            await this.deleteUserCommand.ExecuteAsync(deleteUserCommandRequest);

            //Then
            this.mockUserReadRepository.Verify(s => s.GetAsync(It.IsAny<Guid>()), Times.Once);
            this.mockUserWriteRepository.Verify(s => s.DeleteAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task UserCommand_DeleteUserAsync_TakesDeleteUserCommandRequest_ThrowsUserNotFoundException()
        {
            //Given
            var deleteUserCommandRequest = new DeleteUserCommandRequest { Id = Guid.NewGuid() };

            this.mockUserReadRepository.Setup(s => s.GetAsync(It.IsAny<Guid>())).ReturnsAsync(null as User);
            this.mockUserWriteRepository.Setup(s => s.DeleteAsync(It.IsAny<Guid>()));

            //When
            await Exceptions<UserNotFoundException>.HandleAsync(async () =>
                await this.deleteUserCommand.ExecuteAsync(deleteUserCommandRequest),
                (ex) => Assert.Equal("User not found", ex.Message)
            );

            //Then
            this.mockUserReadRepository.Verify(s => s.GetAsync(It.IsAny<Guid>()), Times.Once);
            this.mockUserWriteRepository.Verify(s => s.DeleteAsync(It.IsAny<Guid>()), Times.Never);
        }
    }
}