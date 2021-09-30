namespace users_test.Users_logic_tests.UnitTests.Command
{
    using Xunit;
    using System;
    using Moq;
    using users_data.Repositories;
    using users_data.Entities;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using users_logic.Exceptions.Command;
    using users_logic.Exceptions.Validation;
    using users_logic.Logic.Command.CreateUserCommand;
    using users_logic.Facades;

    public class UserCommandCreateUserAsyncTests
    {
        private Mock<IWriteRepository<User>> mockUserWriteRepository;
        private Mock<IReadRepository<User>> mockUserReadRepository;
        private Mock<IUserLogicFacade> mockUserLogicFacade;
        private CreateUserCommand createUserCommand;

        public UserCommandCreateUserAsyncTests()
        {
            this.mockUserWriteRepository = new Mock<IWriteRepository<User>>();
            this.mockUserReadRepository = new Mock<IReadRepository<User>>();
            this.mockUserLogicFacade = new Mock<IUserLogicFacade>();
            this.createUserCommand = new CreateUserCommand(mockUserWriteRepository.Object, mockUserReadRepository.Object, mockUserLogicFacade.Object);
        }

        [Fact]
        public async Task UserCommand_CreateUserAsync_TakesNullCreateUserCommandRequest_ExpectsArgumentNullException()
        {
            //Given
            //When
            //Then
            await Exceptions<ArgumentNullException>.HandleAsync(async () =>
                await this.createUserCommand.ExecuteAsync(null),
                (ex) => Assert.Equal("request", ex.ParamName)
            );
        }

        [Fact]
        public async Task UserCommand_CreateUserAsync_TakesCreateUserCommandRequest_ExpectsCreateUserCommandResponseOnEmptyUserRepository()
        {
            //Given
            Guid responseId = Guid.NewGuid();
            var createUserCommandRequest = new CreateUserCommandRequest
            {
                FirstName = "Bob",
                LastName = "Doe",
                Email = "b.doe@mail.com",
                DateOfBirth = new DateTime(1992, 6, 12)
            };

            this.mockUserReadRepository.Setup(s => s.GetAsync()).ReturnsAsync(null as IEnumerable<User>);
            this.mockUserLogicFacade.Setup(s => s.DoesUserEmailAlreadyExistAsync(null as IEnumerable<User>, createUserCommandRequest.Email))
                .ReturnsAsync(false);
            this.mockUserLogicFacade.Setup(s => s.GetCalculatedUsersAgeAsync(It.IsAny<DateTime>())).ReturnsAsync(21);
            this.mockUserLogicFacade.Setup(s => s.IsAgeValidAsync(It.IsAny<int>())).ReturnsAsync(true);
            this.mockUserWriteRepository.Setup(s => s.CreateAsync(It.IsAny<User>())).ReturnsAsync(responseId);

            //When
            CreateUserCommandResponse actualResponse = await createUserCommand.ExecuteAsync(createUserCommandRequest);

            //Then
            Assert.NotNull(actualResponse);
            Assert.True(actualResponse.Id != Guid.Empty);

            this.mockUserReadRepository.Verify(s => s.GetAsync(), Times.Once);
            this.mockUserLogicFacade.Verify(s => s.DoesUserEmailAlreadyExistAsync(It.IsAny<IEnumerable<User>>(), It.IsAny<string>()), Times.Once);
            this.mockUserLogicFacade.Verify(s => s.GetCalculatedUsersAgeAsync(It.IsAny<DateTime>()), Times.Once);
            this.mockUserLogicFacade.Verify(s => s.IsAgeValidAsync(It.IsAny<int>()), Times.Once);
            this.mockUserWriteRepository.Verify(s => s.CreateAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task UserCommand_CreateUserAsync_TakesExistingEmailInCreateUserCommandRequest_ExpectsCommandRequestException()
        {
            //Given
            var createUserCommandRequest = new CreateUserCommandRequest
            {
                FirstName = "Bob",
                LastName = "Doe",
                Email = "b.doe@mail.com",
                DateOfBirth = new DateTime(1992, 6, 12)
            };

            this.mockUserReadRepository.Setup(s => s.GetAsync()).ReturnsAsync(It.IsAny<IEnumerable<User>>());
            this.mockUserLogicFacade.Setup(s => s.DoesUserEmailAlreadyExistAsync(It.IsAny<IEnumerable<User>>(), createUserCommandRequest.Email))
                .ReturnsAsync(true);

            //When
            //Then
            await Exceptions<EmailExistsException>.HandleAsync(async ()
                => await createUserCommand.ExecuteAsync(createUserCommandRequest),
                (ex) => Assert.Equal("Email already exists", ex.Message)
            );

            this.mockUserReadRepository.Verify(s => s.GetAsync(), Times.Once);
            this.mockUserLogicFacade.Verify(s => s.DoesUserEmailAlreadyExistAsync(It.IsAny<IEnumerable<User>>(), It.IsAny<string>()), Times.Once);
            this.mockUserLogicFacade.Verify(s => s.GetCalculatedUsersAgeAsync(It.IsAny<DateTime>()), Times.Never);
            this.mockUserLogicFacade.Verify(s => s.IsAgeValidAsync(It.IsAny<int>()), Times.Never);
            this.mockUserWriteRepository.Verify(s => s.CreateAsync(It.IsAny<User>()), Times.Never);
        }

        [Theory]
        [InlineData(17)]
        [InlineData(111)]
        [InlineData(1990)]
        [InlineData(-1)]
        [InlineData(10000)]
        public async Task UserCommand_CreateUserAsync_TakesInvalidDateOfBirthInCreateUserCommandRequest_ExpectsCommandRequestException(int mockTestAge)
        {
            //Given
            var createUserCommandRequest = new CreateUserCommandRequest
            {
                FirstName = "Bob",
                LastName = "Doe",
                Email = "b.doe@mail.com",
                DateOfBirth = new DateTime(1992, 6, 12)
            };

            this.mockUserReadRepository.Setup(s => s.GetAsync()).ReturnsAsync(It.IsAny<IEnumerable<User>>());
            this.mockUserLogicFacade.Setup(s => s.DoesUserEmailAlreadyExistAsync(It.IsAny<IEnumerable<User>>(), createUserCommandRequest.Email))
                .ReturnsAsync(false);
            this.mockUserLogicFacade.Setup(s => s.GetCalculatedUsersAgeAsync(It.IsAny<DateTime>())).ReturnsAsync(mockTestAge);
            this.mockUserLogicFacade.Setup(s => s.IsAgeValidAsync(It.IsAny<int>())).ReturnsAsync(false);

            //When
            //Then
            await Exceptions<InvalidAgeException>.HandleAsync(async ()
                => await this.createUserCommand.ExecuteAsync(createUserCommandRequest),
                (ex) => Assert.Equal("Ages 18 to 110 can only make a user!", ex.Message)
            );

            this.mockUserReadRepository.Verify(s => s.GetAsync(), Times.Once);
            this.mockUserLogicFacade.Verify(s => s.DoesUserEmailAlreadyExistAsync(It.IsAny<IEnumerable<User>>(), It.IsAny<string>()), Times.Once);
            this.mockUserLogicFacade.Verify(s => s.GetCalculatedUsersAgeAsync(It.IsAny<DateTime>()), Times.Once);
            this.mockUserLogicFacade.Verify(s => s.IsAgeValidAsync(It.IsAny<int>()), Times.Once);
            this.mockUserWriteRepository.Verify(s => s.CreateAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task UserCommand_CreateUserAsync_TakesCreateUserCommandRequest_ReturnsEmptyGuidExpectsCommandRequestException()
        {
            //Given
            var createUserCommandRequest = new CreateUserCommandRequest
            {
                FirstName = "Bob",
                LastName = "Doe",
                Email = "b.doe@mail.com",
                DateOfBirth = new DateTime(1992, 6, 12)
            };

            this.mockUserReadRepository.Setup(s => s.GetAsync()).ReturnsAsync(It.IsAny<IEnumerable<User>>());
            this.mockUserLogicFacade.Setup(s => s.DoesUserEmailAlreadyExistAsync(It.IsAny<IEnumerable<User>>(), createUserCommandRequest.Email))
                .ReturnsAsync(false);
            this.mockUserLogicFacade.Setup(s => s.GetCalculatedUsersAgeAsync(It.IsAny<DateTime>())).ReturnsAsync(21);
            this.mockUserLogicFacade.Setup(s => s.IsAgeValidAsync(It.IsAny<int>())).ReturnsAsync(true);
            this.mockUserWriteRepository.Setup(s => s.CreateAsync(It.IsAny<User>())).ReturnsAsync(Guid.Empty);

            //When
            //Then
            await Exceptions<CommandResponseException>.HandleAsync(async ()
                => await this.createUserCommand.ExecuteAsync(createUserCommandRequest),
                (ex) => Assert.Equal("Response Id was empty", ex.Message)
            );

            this.mockUserReadRepository.Verify(s => s.GetAsync(), Times.Once);
            this.mockUserLogicFacade.Verify(s => s.DoesUserEmailAlreadyExistAsync(It.IsAny<IEnumerable<User>>(), It.IsAny<string>()), Times.Once);
            this.mockUserLogicFacade.Verify(s => s.GetCalculatedUsersAgeAsync(It.IsAny<DateTime>()), Times.Once);
            this.mockUserLogicFacade.Verify(s => s.IsAgeValidAsync(It.IsAny<int>()), Times.Once);
            this.mockUserWriteRepository.Verify(s => s.CreateAsync(It.IsAny<User>()), Times.Once);
        }
    }
}