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
    using users_logic.Logic.Command.UpdateUserCommand;
    using users_logic.Facades;
    using users_logic.Exceptions.UserExceptions;
    using users_test.Helper;

    public class UserCommandUpdateUserAsyncTests
    {
        private UpdateUserCommand updateUserCommand;
        private Mock<IWriteRepository<User>> mockUserWriteRepository;
        private Mock<IReadRepository<User>> mockUserReadRepository;
        private Mock<IUserLogicFacade> mockUserLogicFacade;

        public readonly static Guid mockRequestId = Guid.NewGuid();

        public static UpdateUserCommandRequest updateUserCommandRequest => new UpdateUserCommandRequest
        {
            Id = mockRequestId,
            FirstName = "Jamie",
            LastName = "Haa",
            Email = "newj.haa@email.com",
            DateOfBirth = new DateTime(1993, 12, 01)
        };

        public static IList<User> userRecordsTestData = new List<User>
        {
            new User
            {
                Id = UserCommandUpdateUserAsyncTests.mockRequestId,
                Firstname = "Billy",
                Lastname = "Boe",
                Email = "b.boe@mail.co.uk",
                DateOfBirth = new DateTime(2002, 12, 01),
                Age = 20
            },
            new User
            {
                Id = Guid.NewGuid(),
                Firstname = "James",
                Lastname = "huaah",
                Email = "j.haa@email.com",
                DateOfBirth = new DateTime(1990, 12, 01),
                Age = 28
            }
        };

        public UserCommandUpdateUserAsyncTests()
        {
            this.mockUserWriteRepository = new Mock<IWriteRepository<User>>();
            this.mockUserReadRepository = new Mock<IReadRepository<User>>();
            this.mockUserLogicFacade = new Mock<IUserLogicFacade>();
            this.updateUserCommand = new UpdateUserCommand(this.mockUserWriteRepository.Object, this.mockUserReadRepository.Object, this.mockUserLogicFacade.Object);
        }

        [Fact]
        public async Task UserCommand_UpdateUserAsync_TakesNullUpdateUserCommandRequest_ExpectsArgumentNullException()
        {
            //Given
            //When
            //Then
            await Exceptions<ArgumentNullException>.HandleAsync(async () =>
                await this.updateUserCommand.ExecuteAsync(null),
                (ex) => Assert.Equal("request", ex.ParamName)
            );
        }

        [Fact]
        public async Task UserCommand_UpdateUserAsync_TakesUpdateUserCommandRequest_AndReturnsGuid()
        {
            //Given
            this.mockUserReadRepository.Setup(s => s.GetAsync(It.IsAny<Guid>())).ReturnsAsync(userRecordsTestData[0]);
            this.mockUserReadRepository.Setup(s => s.GetAsync()).ReturnsAsync(userRecordsTestData);
            this.mockUserLogicFacade.Setup(s => s.DoesUserEmailAlreadyExistAsync(It.IsAny<IEnumerable<User>>(), It.IsAny<string>()))
                .ReturnsAsync(false);
            this.mockUserLogicFacade.Setup(s => s.GetCalculatedUsersAgeAsync(It.IsAny<DateTime>())).ReturnsAsync(21);
            this.mockUserLogicFacade.Setup(s => s.IsAgeValidAsync(It.IsAny<int>())).ReturnsAsync(true);
            this.mockUserWriteRepository.Setup(s => s.UpdateAsync(It.IsAny<User>())).ReturnsAsync(userRecordsTestData[0].Id);

            //When
            UpdateUserCommandResponse actualResponse = await this.updateUserCommand.ExecuteAsync(updateUserCommandRequest);

            //Then
            Assert.NotNull(actualResponse);
            Assert.True(actualResponse.Id != Guid.Empty);
            Assert.Equal(mockRequestId, actualResponse.Id);

            this.mockUserReadRepository.Verify(s => s.GetAsync(It.IsAny<Guid>()), Times.Once);
            this.mockUserReadRepository.Verify(s => s.GetAsync(), Times.Once);
            this.mockUserLogicFacade.Verify(s => s.DoesUserEmailAlreadyExistAsync(It.IsAny<IEnumerable<User>>(), It.IsAny<string>()), Times.Once);
            this.mockUserLogicFacade.Verify(s => s.GetCalculatedUsersAgeAsync(It.IsAny<DateTime>()), Times.Once);
            this.mockUserLogicFacade.Verify(s => s.IsAgeValidAsync(It.IsAny<int>()), Times.Once);
            this.mockUserWriteRepository.Verify(s => s.UpdateAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task UserCommand_UpdateUserAsync_TakesUpdateUserCommandRequestWithNewEmail_AndThrowsCommandRequestException()
        {
            //Given
            this.mockUserReadRepository.Setup(s => s.GetAsync(It.IsAny<Guid>())).ReturnsAsync(userRecordsTestData[0]);
            this.mockUserReadRepository.Setup(s => s.GetAsync()).ReturnsAsync(userRecordsTestData);
            this.mockUserLogicFacade.Setup(s => s.DoesUserEmailAlreadyExistAsync(It.IsAny<IEnumerable<User>>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            //When
            await Exceptions<EmailExistsException>.HandleAsync(async ()
                => await this.updateUserCommand.ExecuteAsync(updateUserCommandRequest),
                (ex) => Assert.Equal("Email already exists", ex.Message)
            );

            //Then
            this.mockUserReadRepository.Verify(s => s.GetAsync(It.IsAny<Guid>()), Times.Once);
            this.mockUserReadRepository.Verify(s => s.GetAsync(), Times.Once);
            this.mockUserLogicFacade.Verify(s => s.DoesUserEmailAlreadyExistAsync(It.IsAny<IEnumerable<User>>(), It.IsAny<string>()), Times.Once);
            this.mockUserLogicFacade.Verify(s => s.GetCalculatedUsersAgeAsync(It.IsAny<DateTime>()), Times.Never);
            this.mockUserLogicFacade.Verify(s => s.IsAgeValidAsync(It.IsAny<int>()), Times.Never);
            this.mockUserWriteRepository.Verify(s => s.UpdateAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task UserCommand_UpdateUserAsync_TakesUpdateUserCommandRequestWithNewEmail_AndReturnsGuid()
        {
            //Given
            this.mockUserReadRepository.Setup(s => s.GetAsync(It.IsAny<Guid>())).ReturnsAsync(userRecordsTestData[0]);
            this.mockUserReadRepository.Setup(s => s.GetAsync()).ReturnsAsync(userRecordsTestData);
            this.mockUserLogicFacade.Setup(s => s.DoesUserEmailAlreadyExistAsync(It.IsAny<IEnumerable<User>>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            //When
            await Exceptions<EmailExistsException>.HandleAsync(async ()
                => await this.updateUserCommand.ExecuteAsync(updateUserCommandRequest),
                (ex) => Assert.Equal("Email already exists", ex.Message)
            );

            //Then
            this.mockUserReadRepository.Verify(s => s.GetAsync(It.IsAny<Guid>()), Times.Once);
            this.mockUserReadRepository.Verify(s => s.GetAsync(), Times.Once);
            this.mockUserLogicFacade.Verify(s => s.DoesUserEmailAlreadyExistAsync(It.IsAny<IEnumerable<User>>(), It.IsAny<string>()), Times.Once);
            this.mockUserLogicFacade.Verify(s => s.GetCalculatedUsersAgeAsync(It.IsAny<DateTime>()), Times.Never);
            this.mockUserLogicFacade.Verify(s => s.IsAgeValidAsync(It.IsAny<int>()), Times.Never);
            this.mockUserWriteRepository.Verify(s => s.UpdateAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task UserCommand_UpdateUserAsync_TakesUpdateUserCommandRequestWithInvalidDateOfBirth_AndThrowsCommandRequestException()
        {
            //Given
            this.mockUserReadRepository.Setup(s => s.GetAsync(It.IsAny<Guid>())).ReturnsAsync(userRecordsTestData[0]);
            this.mockUserReadRepository.Setup(s => s.GetAsync()).ReturnsAsync(userRecordsTestData);
            this.mockUserLogicFacade.Setup(s => s.DoesUserEmailAlreadyExistAsync(It.IsAny<IEnumerable<User>>(), It.IsAny<string>()))
                .ReturnsAsync(false);
            this.mockUserLogicFacade.Setup(s => s.GetCalculatedUsersAgeAsync(It.IsAny<DateTime>())).ReturnsAsync(17);
            this.mockUserLogicFacade.Setup(s => s.IsAgeValidAsync(It.IsAny<int>())).ReturnsAsync(false);

            //When
            await Exceptions<InvalidDateOfBirthException>.HandleAsync(async ()
                => await this.updateUserCommand.ExecuteAsync(updateUserCommandRequest),
                (ex) => Assert.Equal("Invalid date of birth", ex.Message)
            );

            //Then
            this.mockUserReadRepository.Verify(s => s.GetAsync(It.IsAny<Guid>()), Times.Once);
            this.mockUserReadRepository.Verify(s => s.GetAsync(), Times.Once);
            this.mockUserLogicFacade.Verify(s => s.DoesUserEmailAlreadyExistAsync(It.IsAny<IEnumerable<User>>(), It.IsAny<string>()), Times.Once);
            this.mockUserLogicFacade.Verify(s => s.GetCalculatedUsersAgeAsync(It.IsAny<DateTime>()), Times.Once);
            this.mockUserLogicFacade.Verify(s => s.IsAgeValidAsync(It.IsAny<int>()), Times.Once);
            this.mockUserWriteRepository.Verify(s => s.UpdateAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task UserCommand_UpdateUserAsync_TakesUpdateUserCommandRequest_AndThrowsUserNotFoundException()
        {
            //Given
            this.mockUserReadRepository.Setup(s => s.GetAsync(It.IsAny<Guid>())).ReturnsAsync(userRecordsTestData[0]);
            this.mockUserReadRepository.Setup(s => s.GetAsync()).ReturnsAsync(userRecordsTestData);
            this.mockUserLogicFacade.Setup(s => s.DoesUserEmailAlreadyExistAsync(It.IsAny<IEnumerable<User>>(), It.IsAny<string>()))
                .ReturnsAsync(false);
            this.mockUserLogicFacade.Setup(s => s.GetCalculatedUsersAgeAsync(It.IsAny<DateTime>())).ReturnsAsync(17);
            this.mockUserReadRepository.Setup(s => s.GetAsync(It.IsAny<Guid>())).ReturnsAsync(null as User);

            //When
            await Exceptions<UserNotFoundException>.HandleAsync(async ()
                => await this.updateUserCommand.ExecuteAsync(updateUserCommandRequest),
                (ex) => Assert.Equal("User not found", ex.Message)
            );

            //Then
            this.mockUserReadRepository.Verify(s => s.GetAsync(It.IsAny<Guid>()), Times.Once);
            this.mockUserReadRepository.Verify(s => s.GetAsync(), Times.Never);
            this.mockUserLogicFacade.Verify(s => s.DoesUserEmailAlreadyExistAsync(It.IsAny<IEnumerable<User>>(), It.IsAny<string>()), Times.Never);
            this.mockUserLogicFacade.Verify(s => s.GetCalculatedUsersAgeAsync(It.IsAny<DateTime>()), Times.Never);
            this.mockUserLogicFacade.Verify(s => s.IsAgeValidAsync(It.IsAny<int>()), Times.Never);
            this.mockUserWriteRepository.Verify(s => s.UpdateAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task UserCommand_UpdateUserAsync_TakesUpdateUserCommandRequest_AndThrowsCommandResponseException()
        {
            //Given
            this.mockUserReadRepository.Setup(s => s.GetAsync(It.IsAny<Guid>())).ReturnsAsync(userRecordsTestData[0]);
            this.mockUserReadRepository.Setup(s => s.GetAsync()).ReturnsAsync(userRecordsTestData);
            this.mockUserLogicFacade.Setup(s => s.DoesUserEmailAlreadyExistAsync(It.IsAny<IEnumerable<User>>(), It.IsAny<string>()))
                .ReturnsAsync(false);
            this.mockUserLogicFacade.Setup(s => s.GetCalculatedUsersAgeAsync(It.IsAny<DateTime>())).ReturnsAsync(21);
            this.mockUserLogicFacade.Setup(s => s.IsAgeValidAsync(It.IsAny<int>())).ReturnsAsync(true);
            this.mockUserWriteRepository.Setup(s => s.UpdateAsync(It.IsAny<User>())).ReturnsAsync(Guid.Empty);

            //When
            await Exceptions<CommandResponseException>.HandleAsync(async ()
                => await this.updateUserCommand.ExecuteAsync(updateUserCommandRequest),
                (ex) => Assert.Equal("Response Id was empty", ex.Message)
            );

            //Then
            this.mockUserReadRepository.Verify(s => s.GetAsync(It.IsAny<Guid>()), Times.Once);
            this.mockUserReadRepository.Verify(s => s.GetAsync(), Times.Once);
            this.mockUserLogicFacade.Verify(s => s.DoesUserEmailAlreadyExistAsync(It.IsAny<IEnumerable<User>>(), It.IsAny<string>()), Times.Once);
            this.mockUserLogicFacade.Verify(s => s.GetCalculatedUsersAgeAsync(It.IsAny<DateTime>()), Times.Once);
            this.mockUserLogicFacade.Verify(s => s.IsAgeValidAsync(It.IsAny<int>()), Times.Once);
            this.mockUserWriteRepository.Verify(s => s.UpdateAsync(It.IsAny<User>()), Times.Once);
        }
    }
}