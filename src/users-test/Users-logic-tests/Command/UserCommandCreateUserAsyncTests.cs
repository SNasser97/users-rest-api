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
    using users_logic.User.Logic.Command.Models.Response;
    using System.Collections.Generic;
    using users_logic.Exceptions.Command;
    using users_logic.Exceptions.Validation;
    using users_logic.User.Exceptions.Validation;

    public class UserCommandCreateUserAsyncTests
    {
        private Mock<IWriteRepository<BaseUserRecord, BaseUserRecordWithId>> mockUserWriteRepository;
        private Mock<IReadRepository<BaseUserRecordWithId>> mockUserReadRepository;
        private Mock<IUserLogicFacade> mockUserLogicFacade;
        private UserCommand userCommand;

        public UserCommandCreateUserAsyncTests()
        {
            this.mockUserWriteRepository = new Mock<IWriteRepository<BaseUserRecord, BaseUserRecordWithId>>();
            this.mockUserReadRepository = new Mock<IReadRepository<BaseUserRecordWithId>>();
            this.mockUserLogicFacade = new Mock<IUserLogicFacade>();
            this.userCommand = new UserCommand(mockUserWriteRepository.Object, mockUserReadRepository.Object, mockUserLogicFacade.Object);
        }

        [Fact]
        public async Task UserCommand_CreateUserAsync_TakesNullCreateUserCommandRequest_ExpectsArgumentNullException()
        {
            //Given
            //When
            //Then
            await Exceptions<ArgumentNullException>.HandleAsync(async () =>
                await this.userCommand.CreateUserAsync(null),
                (ex) => Assert.Equal("request", ex.ParamName)
            );
        }

        [Fact]
        public async Task UserCommand_CreateUserAsync_TakesCreateUserCommandRequest_ExpectsCreateUserCommandResponseOnEmptyUserRepository()
        {
            //Given
            Guid responseId = Guid.NewGuid();
            var createUserCommandRequest = new CreateUserCommandRequestModel
            {
                FirstName = "Bob",
                LastName = "Doe",
                Email = "b.doe@mail.com",
                DateOfBirth = new DateTime(1992, 6, 12)
            };

            this.mockUserReadRepository.Setup(s => s.GetAsync()).ReturnsAsync(null as IEnumerable<UserRecord>);
            this.mockUserLogicFacade.Setup(s => s.DoesUserEmailAlreadyExistAsync(null as IEnumerable<UserRecord>, createUserCommandRequest.Email))
                .ReturnsAsync(false);
            this.mockUserLogicFacade.Setup(s => s.GetCalculatedUsersAgeAsync(It.IsAny<DateTime>())).ReturnsAsync(21);
            this.mockUserLogicFacade.Setup(s => s.IsAgeValidAsync(It.IsAny<int>())).ReturnsAsync(true);
            this.mockUserWriteRepository.Setup(s => s.CreateAsync(It.IsAny<CreateUserRecord>())).ReturnsAsync(responseId);

            //When
            CreateUserCommandResponseModel actualResponse = (CreateUserCommandResponseModel)await userCommand.CreateUserAsync(createUserCommandRequest);

            //Then
            Assert.NotNull(actualResponse);
            Assert.True(actualResponse.Id != Guid.Empty);

            this.mockUserReadRepository.Verify(s => s.GetAsync(), Times.Once);
            this.mockUserLogicFacade.Verify(s => s.DoesUserEmailAlreadyExistAsync(It.IsAny<IEnumerable<UserRecord>>(), It.IsAny<string>()), Times.Once);
            this.mockUserLogicFacade.Verify(s => s.GetCalculatedUsersAgeAsync(It.IsAny<DateTime>()), Times.Once);
            this.mockUserLogicFacade.Verify(s => s.IsAgeValidAsync(It.IsAny<int>()), Times.Once);
            this.mockUserWriteRepository.Verify(s => s.CreateAsync(It.IsAny<CreateUserRecord>()), Times.Once);
        }

        [Fact]
        public async Task UserCommand_CreateUserAsync_TakesExistingEmailInCreateUserCommandRequest_ExpectsCommandRequestException()
        {
            //Given
            var createUserCommandRequest = new CreateUserCommandRequestModel
            {
                FirstName = "Bob",
                LastName = "Doe",
                Email = "b.doe@mail.com",
                DateOfBirth = new DateTime(1992, 6, 12)
            };

            this.mockUserReadRepository.Setup(s => s.GetAsync()).ReturnsAsync(It.IsAny<IEnumerable<UserRecord>>());
            this.mockUserLogicFacade.Setup(s => s.DoesUserEmailAlreadyExistAsync(It.IsAny<IEnumerable<UserRecord>>(), createUserCommandRequest.Email))
                .ReturnsAsync(true);

            //When
            //Then
            await Exceptions<EmailExistsException>.HandleAsync(async ()
                => await userCommand.CreateUserAsync(createUserCommandRequest),
                (ex) => Assert.Equal("Email already exists", ex.Message)
            );

            this.mockUserReadRepository.Verify(s => s.GetAsync(), Times.Once);
            this.mockUserLogicFacade.Verify(s => s.DoesUserEmailAlreadyExistAsync(It.IsAny<IEnumerable<UserRecord>>(), It.IsAny<string>()), Times.Once);
            this.mockUserLogicFacade.Verify(s => s.GetCalculatedUsersAgeAsync(It.IsAny<DateTime>()), Times.Never);
            this.mockUserLogicFacade.Verify(s => s.IsAgeValidAsync(It.IsAny<int>()), Times.Never);
            this.mockUserWriteRepository.Verify(s => s.CreateAsync(It.IsAny<CreateUserRecord>()), Times.Never);
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
            var createUserCommandRequest = new CreateUserCommandRequestModel
            {
                FirstName = "Bob",
                LastName = "Doe",
                Email = "b.doe@mail.com",
                DateOfBirth = new DateTime(1992, 6, 12)
            };

            this.mockUserReadRepository.Setup(s => s.GetAsync()).ReturnsAsync(It.IsAny<IEnumerable<UserRecord>>());
            this.mockUserLogicFacade.Setup(s => s.DoesUserEmailAlreadyExistAsync(It.IsAny<IEnumerable<UserRecord>>(), createUserCommandRequest.Email))
                .ReturnsAsync(false);
            this.mockUserLogicFacade.Setup(s => s.GetCalculatedUsersAgeAsync(It.IsAny<DateTime>())).ReturnsAsync(mockTestAge);
            this.mockUserLogicFacade.Setup(s => s.IsAgeValidAsync(It.IsAny<int>())).ReturnsAsync(false);

            //When
            //Then
            await Exceptions<InvalidAgeException>.HandleAsync(async ()
                => await this.userCommand.CreateUserAsync(createUserCommandRequest),
                (ex) => Assert.Equal("Ages 18 to 110 can only make a user!", ex.Message)
            );

            this.mockUserReadRepository.Verify(s => s.GetAsync(), Times.Once);
            this.mockUserLogicFacade.Verify(s => s.DoesUserEmailAlreadyExistAsync(It.IsAny<IEnumerable<UserRecord>>(), It.IsAny<string>()), Times.Once);
            this.mockUserLogicFacade.Verify(s => s.GetCalculatedUsersAgeAsync(It.IsAny<DateTime>()), Times.Once);
            this.mockUserLogicFacade.Verify(s => s.IsAgeValidAsync(It.IsAny<int>()), Times.Once);
            this.mockUserWriteRepository.Verify(s => s.CreateAsync(It.IsAny<CreateUserRecord>()), Times.Never);
        }

        [Fact]
        public async Task UserCommand_CreateUserAsync_TakesCreateUserCommandRequest_ReturnsEmptyGuidExpectsCommandRequestException()
        {
            //Given
            var createUserCommandRequest = new CreateUserCommandRequestModel
            {
                FirstName = "Bob",
                LastName = "Doe",
                Email = "b.doe@mail.com",
                DateOfBirth = new DateTime(1992, 6, 12)
            };

            this.mockUserReadRepository.Setup(s => s.GetAsync()).ReturnsAsync(It.IsAny<IEnumerable<UserRecord>>());
            this.mockUserLogicFacade.Setup(s => s.DoesUserEmailAlreadyExistAsync(It.IsAny<IEnumerable<UserRecord>>(), createUserCommandRequest.Email))
                .ReturnsAsync(false);
            this.mockUserLogicFacade.Setup(s => s.GetCalculatedUsersAgeAsync(It.IsAny<DateTime>())).ReturnsAsync(21);
            this.mockUserLogicFacade.Setup(s => s.IsAgeValidAsync(It.IsAny<int>())).ReturnsAsync(true);
            this.mockUserWriteRepository.Setup(s => s.CreateAsync(It.IsAny<UserRecord>())).ReturnsAsync(Guid.Empty);

            //When
            //Then
            await Exceptions<CommandResponseException>.HandleAsync(async ()
                => await this.userCommand.CreateUserAsync(createUserCommandRequest),
                (ex) => Assert.Equal("Response Id was empty", ex.Message)
            );

            this.mockUserReadRepository.Verify(s => s.GetAsync(), Times.Once);
            this.mockUserLogicFacade.Verify(s => s.DoesUserEmailAlreadyExistAsync(It.IsAny<IEnumerable<UserRecord>>(), It.IsAny<string>()), Times.Once);
            this.mockUserLogicFacade.Verify(s => s.GetCalculatedUsersAgeAsync(It.IsAny<DateTime>()), Times.Once);
            this.mockUserLogicFacade.Verify(s => s.IsAgeValidAsync(It.IsAny<int>()), Times.Once);
            this.mockUserWriteRepository.Verify(s => s.CreateAsync(It.IsAny<CreateUserRecord>()), Times.Once);
        }
    }
}