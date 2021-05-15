namespace users_test.Users_data_tests.Repositories.InMemoryUserWriteRepository
{
    using System.Linq;
    using System.Threading.Tasks;
    using users_data.Repositories.InMemoryUserRepository;
    using Xunit;
    using System.Collections.Generic;
    using System;
    using users_data.Entities;

    public class InMemoryUserWriteRepositoryCreateAsyncTests
    {
        [Fact]
        public async Task InMemoryUserWriteRepository_CreateAsync_ReturnsAGuid()
        {
            //Given
            var newUser = new UserRecord
            {
                Id = Guid.NewGuid(),
                FirstName = "Bob",
                LastName = "Doe",
                Email = "bdoe@mail.com",
                DateOfBirth = new DateTime(1997, 12, 06),
                Age = 24
            };

            var users = new Dictionary<Guid, UserRecord>
            {
                { Guid.NewGuid(), new UserRecord() },
                { Guid.NewGuid(), new UserRecord() },
                { Guid.NewGuid(), new UserRecord() },
            };

            var usersWriteRepository = new InMemoryUserWriteRepository(users);

            //When
            Guid actualGuid = await usersWriteRepository.CreateAsync(newUser);

            //Then
            Assert.True(actualGuid != Guid.Empty);
            Assert.Equal(4, users.Count());

            KeyValuePair<Guid, UserRecord> actualUserKeyValuePair = users.FirstOrDefault(u => u.Key == actualGuid);
            Assert.NotEqual(default(KeyValuePair<Guid, UserRecord>), actualUserKeyValuePair);
            Assert.True(actualUserKeyValuePair.Key != Guid.Empty);
            Assert.Equal(actualUserKeyValuePair.Key, actualGuid);

            UserRecord actualNewUser = actualUserKeyValuePair.Value;
            Assert.NotNull(actualUserKeyValuePair.Value);
            Assert.Equal(newUser.Id, actualNewUser.Id);
            Assert.Equal(newUser.FirstName, actualNewUser.FirstName);
            Assert.Equal(newUser.LastName, actualNewUser.LastName);
            Assert.Equal(newUser.Email, actualNewUser.Email);
            Assert.Equal(newUser.DateOfBirth, actualNewUser.DateOfBirth);
            Assert.Equal(newUser.Age, actualNewUser.Age);
        }

        [Fact]
        public async Task InMemoryUserWriteRepository_CreateAsync_TakesExistingGuidReturnsEmptyGuid()
        {
            //Given
            var newUser = new UserRecord
            {
                Id = Guid.NewGuid(),
                FirstName = "All",
                LastName = "Jann",
                Email = "all.jann@mail.com",
                DateOfBirth = new DateTime(2001, 12, 06),
                Age = 24
            };

            var existingUser = new UserRecord
            {
                Id = newUser.Id,
                FirstName = "Bob",
                LastName = "Doe",
                Email = "bdoe@mail.com",
                DateOfBirth = new DateTime(1997, 12, 06),
                Age = 35
            };

            var users = new Dictionary<Guid, UserRecord>
            {
                { existingUser.Id, existingUser },
                { Guid.NewGuid(), new UserRecord() },
                { Guid.NewGuid(), new UserRecord() },
            };

            var usersWriteRepository = new InMemoryUserWriteRepository(users);

            //When
            Guid actualGuid = await usersWriteRepository.CreateAsync(newUser);

            //Then
            Assert.True(actualGuid == Guid.Empty);
            Assert.Equal(3, users.Count());
        }
    }
}