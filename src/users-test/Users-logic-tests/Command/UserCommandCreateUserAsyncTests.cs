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

        [Fact]
        public async Task UserCommand_CreateUserAsync_TakesNullCreateUserCommandRequest_ExpectsArgumentNullException()
        {
            //Given
            var mockUserWriteRepository = new Mock<IWriteRepository<BaseUserRecord, BaseUserRecordWithId>>();
            var mockUserReadRepository = new Mock<IReadRepository<BaseUserRecordWithId>>();
            var mockUserLogicFacade = new Mock<IUserLogicFacade>();
            var userCommand = new UserCommand(mockUserWriteRepository.Object, mockUserReadRepository.Object, mockUserLogicFacade.Object);

            //When
            //Then
            await Exceptions<ArgumentNullException>.HandleAsync(async () =>
                await userCommand.CreateUserAsync(null),
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

            var mockUserWriteRepository = new Mock<IWriteRepository<BaseUserRecord, BaseUserRecordWithId>>();
            var mockUserReadRepository = new Mock<IReadRepository<BaseUserRecordWithId>>();
            var mockUserLogicFacade = new Mock<IUserLogicFacade>();
            var userCommand = new UserCommand(mockUserWriteRepository.Object, mockUserReadRepository.Object, mockUserLogicFacade.Object);

            mockUserReadRepository.Setup(s => s.GetAsync()).ReturnsAsync(null as IEnumerable<UserRecord>);
            mockUserLogicFacade.Setup(s => s.DoesUserEmailAlreadyExistAsync(null as IEnumerable<UserRecord>, createUserCommandRequest.Email))
                .ReturnsAsync(false);
            mockUserLogicFacade.Setup(s => s.GetCalculatedUsersAgeAsync(It.IsAny<DateTime>())).ReturnsAsync(21);
            mockUserLogicFacade.Setup(s => s.IsAgeValidAsync(It.IsAny<int>())).ReturnsAsync(true);
            mockUserWriteRepository.Setup(s => s.CreateAsync(It.IsAny<CreateUserRecord>())).ReturnsAsync(responseId);
            //When
            CreateUserCommandResponse actualResponse = (CreateUserCommandResponse)await userCommand.CreateUserAsync(createUserCommandRequest);

            //Then
            Assert.NotNull(actualResponse);
            Assert.True(actualResponse.Id != Guid.Empty);

            mockUserReadRepository.Verify(s => s.GetAsync(), Times.Once);
            mockUserLogicFacade.Verify(s => s.DoesUserEmailAlreadyExistAsync(It.IsAny<IEnumerable<UserRecord>>(), It.IsAny<string>()), Times.Once);
            mockUserLogicFacade.Verify(s => s.GetCalculatedUsersAgeAsync(It.IsAny<DateTime>()), Times.Once);
            mockUserLogicFacade.Verify(s => s.IsAgeValidAsync(It.IsAny<int>()), Times.Once);
            mockUserWriteRepository.Verify(s => s.CreateAsync(It.IsAny<CreateUserRecord>()), Times.Once);
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

            var mockUserWriteRepository = new Mock<IWriteRepository<BaseUserRecord, BaseUserRecordWithId>>();
            var mockUserReadRepository = new Mock<IReadRepository<BaseUserRecordWithId>>();
            var mockUserLogicFacade = new Mock<IUserLogicFacade>();
            var userCommand = new UserCommand(mockUserWriteRepository.Object, mockUserReadRepository.Object, mockUserLogicFacade.Object);

            mockUserReadRepository.Setup(s => s.GetAsync()).ReturnsAsync(It.IsAny<IEnumerable<UserRecord>>());
            mockUserLogicFacade.Setup(s => s.DoesUserEmailAlreadyExistAsync(It.IsAny<IEnumerable<UserRecord>>(), createUserCommandRequest.Email))
                .ReturnsAsync(true);

            //When
            //Then
            await Exceptions<EmailExistsException>.HandleAsync(async ()
                => await userCommand.CreateUserAsync(createUserCommandRequest),
                (ex) => Assert.Equal("Email already exists", ex.Message)
            );

            mockUserReadRepository.Verify(s => s.GetAsync(), Times.Once);
            mockUserLogicFacade.Verify(s => s.DoesUserEmailAlreadyExistAsync(It.IsAny<IEnumerable<UserRecord>>(), It.IsAny<string>()), Times.Once);
            mockUserLogicFacade.Verify(s => s.GetCalculatedUsersAgeAsync(It.IsAny<DateTime>()), Times.Never);
            mockUserLogicFacade.Verify(s => s.IsAgeValidAsync(It.IsAny<int>()), Times.Never);
            mockUserWriteRepository.Verify(s => s.CreateAsync(It.IsAny<CreateUserRecord>()), Times.Never);
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

            var mockUserWriteRepository = new Mock<IWriteRepository<BaseUserRecord, BaseUserRecordWithId>>();
            var mockUserReadRepository = new Mock<IReadRepository<BaseUserRecordWithId>>();
            var mockUserLogicFacade = new Mock<IUserLogicFacade>();
            var userCommand = new UserCommand(mockUserWriteRepository.Object, mockUserReadRepository.Object, mockUserLogicFacade.Object);

            mockUserReadRepository.Setup(s => s.GetAsync()).ReturnsAsync(It.IsAny<IEnumerable<UserRecord>>());
            mockUserLogicFacade.Setup(s => s.DoesUserEmailAlreadyExistAsync(It.IsAny<IEnumerable<UserRecord>>(), createUserCommandRequest.Email))
                .ReturnsAsync(false);
            mockUserLogicFacade.Setup(s => s.GetCalculatedUsersAgeAsync(It.IsAny<DateTime>())).ReturnsAsync(mockTestAge);
            mockUserLogicFacade.Setup(s => s.IsAgeValidAsync(It.IsAny<int>())).ReturnsAsync(false);

            //When
            //Then
            await Exceptions<InvalidDateOfBirthException>.HandleAsync(async ()
                => await userCommand.CreateUserAsync(createUserCommandRequest),
                (ex) => Assert.Equal("Ages 18 to 110 can only make a user!", ex.Message)
            );

            mockUserReadRepository.Verify(s => s.GetAsync(), Times.Once);
            mockUserLogicFacade.Verify(s => s.DoesUserEmailAlreadyExistAsync(It.IsAny<IEnumerable<UserRecord>>(), It.IsAny<string>()), Times.Once);
            mockUserLogicFacade.Verify(s => s.GetCalculatedUsersAgeAsync(It.IsAny<DateTime>()), Times.Once);
            mockUserLogicFacade.Verify(s => s.IsAgeValidAsync(It.IsAny<int>()), Times.Once);
            mockUserWriteRepository.Verify(s => s.CreateAsync(It.IsAny<CreateUserRecord>()), Times.Never);
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

            var mockUserWriteRepository = new Mock<IWriteRepository<BaseUserRecord, BaseUserRecordWithId>>();
            var mockUserReadRepository = new Mock<IReadRepository<BaseUserRecordWithId>>();
            var mockUserLogicFacade = new Mock<IUserLogicFacade>();
            var userCommand = new UserCommand(mockUserWriteRepository.Object, mockUserReadRepository.Object, mockUserLogicFacade.Object);

            mockUserReadRepository.Setup(s => s.GetAsync()).ReturnsAsync(It.IsAny<IEnumerable<UserRecord>>());
            mockUserLogicFacade.Setup(s => s.DoesUserEmailAlreadyExistAsync(It.IsAny<IEnumerable<UserRecord>>(), createUserCommandRequest.Email))
                .ReturnsAsync(false);
            mockUserLogicFacade.Setup(s => s.GetCalculatedUsersAgeAsync(It.IsAny<DateTime>())).ReturnsAsync(21);
            mockUserLogicFacade.Setup(s => s.IsAgeValidAsync(It.IsAny<int>())).ReturnsAsync(true);
            mockUserWriteRepository.Setup(s => s.CreateAsync(It.IsAny<UserRecord>())).ReturnsAsync(Guid.Empty);

            //When
            //Then
            await Exceptions<CommandResponseException>.HandleAsync(async ()
                => await userCommand.CreateUserAsync(createUserCommandRequest),
                (ex) => Assert.Equal("Response Id was empty", ex.Message)
            );

            mockUserReadRepository.Verify(s => s.GetAsync(), Times.Once);
            mockUserLogicFacade.Verify(s => s.DoesUserEmailAlreadyExistAsync(It.IsAny<IEnumerable<UserRecord>>(), It.IsAny<string>()), Times.Once);
            mockUserLogicFacade.Verify(s => s.GetCalculatedUsersAgeAsync(It.IsAny<DateTime>()), Times.Once);
            mockUserLogicFacade.Verify(s => s.IsAgeValidAsync(It.IsAny<int>()), Times.Once);
            mockUserWriteRepository.Verify(s => s.CreateAsync(It.IsAny<CreateUserRecord>()), Times.Once);
        }
    }
}