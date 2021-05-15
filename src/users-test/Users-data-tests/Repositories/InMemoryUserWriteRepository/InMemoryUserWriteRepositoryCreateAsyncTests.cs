namespace users_test.Users_data_tests.Repositories.InMemoryUserWriteRepository
{
    using System.Linq;
    using System.Threading.Tasks;
    using users_data.Models;
    using users_data.Repositories.InMemoryUserRepository;
    using Xunit;
    using System.Collections.Generic;
    using System;
    using Moq;
    using users_data.Facades;

    public class InMemoryUserWriteRepositoryCreateAsyncTests
    {
        [Fact]
        public async Task InMemoryUserWriteRepository_CreateAsyncWithUniqueEmail_ReturnsAGuid()
        {
            //Given
            var newUser = new CreateUserRecord
            {
                FirstName = "Bob",
                LastName = "Doe",
                Email = "bdoe@mail.com",
                DateOfBirth = new DateTime(1997, 12, 06)
            };

            var users = new Dictionary<Guid, UserRecord>
            {
                { Guid.NewGuid(), new UserRecord() },
                { Guid.NewGuid(), new UserRecord() },
                { Guid.NewGuid(), new UserRecord() },
            };

            var mockUserFacade = new Mock<IUserFacade>();

            var usersWriteRepository = new InMemoryUserWriteRepository(users, mockUserFacade.Object);
            mockUserFacade.Setup(s => s.CanUserBeInsertedAsync(It.IsAny<IEnumerable<UserRecord>>(), It.IsAny<CreateUserRecord>()))
                .ReturnsAsync(true);
            mockUserFacade.Setup(s => s.GetUserAgeAsync(It.IsAny<DateTime>())).ReturnsAsync(23);

            //When
            Guid actualGuid = await usersWriteRepository.CreateAsync(newUser);

            //Then
            Assert.True(actualGuid != Guid.Empty);
            Assert.Equal(4, users.Count());

            KeyValuePair<Guid, UserRecord> exists = users.FirstOrDefault(u => u.Key == actualGuid);
            Assert.NotEqual(new KeyValuePair<Guid, UserRecord>(), exists);
            Assert.Equal(exists.Key, actualGuid);
        }

        [Fact]
        public async Task InMemoryUserWriteRepository_CreateAsyncWithExistingEmail_ReturnsEmptyGuid()
        {
            //Given
            var newUser = new CreateUserRecord
            {
                FirstName = "Bob",
                LastName = "Doe",
                Email = "bdoe@mail.com",
                DateOfBirth = new DateTime(1997, 12, 06)
            };

            var existingUser = new UserRecord
            {
                Id = Guid.NewGuid(),
                FirstName = "Bob",
                LastName = "Doe",
                Email = "bdoe@mail.com",
                DateOfBirth = new DateTime(1997, 12, 06)
            };

            var users = new Dictionary<Guid, UserRecord>
            {
                { existingUser.Id, existingUser },
                { Guid.NewGuid(), new UserRecord() },
                { Guid.NewGuid(), new UserRecord() },
            };

            var mockUserFacade = new Mock<IUserFacade>();

            var usersWriteRepository = new InMemoryUserWriteRepository(users, mockUserFacade.Object);
            mockUserFacade.Setup(s => s.CanUserBeInsertedAsync(It.IsAny<IEnumerable<UserRecord>>(), It.IsAny<CreateUserRecord>()))
               .ReturnsAsync(false);
            mockUserFacade.Setup(s => s.GetUserAgeAsync(It.IsAny<DateTime>())).ReturnsAsync(23);

            //When
            Guid actualGuid = await usersWriteRepository.CreateAsync(newUser);

            //Then
            Assert.True(actualGuid == Guid.Empty);
            Assert.Equal(3, users.Count());
        }
    }
}