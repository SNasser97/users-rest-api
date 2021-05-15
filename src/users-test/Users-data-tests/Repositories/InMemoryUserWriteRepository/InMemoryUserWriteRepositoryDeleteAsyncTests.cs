namespace users_test.Users_data_tests.Repositories.InMemoryUserWriteRepository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using users_data.Entities;
    using users_data.Repositories.InMemoryUserRepository;
    using Xunit;

    public class InMemoryUserWriteRepositoryDeleteAsyncTests
    {
        [Fact]
        public async Task InMemoryUserWriteRepository_DeleteAsync_TakesExistingUserGuidAndRemovesUser()
        {
            //Given
            Guid userId = Guid.NewGuid();

            var existingUser = new UserRecord
            {
                Id = userId,
                FirstName = "Bob",
                LastName = "Doe",
                Email = "bdoe@mail.com",
                DateOfBirth = new DateTime(1997, 12, 06),
                Age = 23
            };

            var users = new Dictionary<Guid, UserRecord>
            {
                { Guid.NewGuid(), new UserRecord() },
                { existingUser.Id, existingUser},
                { Guid.NewGuid(), new UserRecord() },
                { Guid.NewGuid(), new UserRecord() },
            };

            var usersWriteRepository = new InMemoryUserWriteRepository(users);

            //When
            await usersWriteRepository.DeleteAsync(userId);

            //Then
            Assert.NotEmpty(users.Keys);
            Assert.NotEmpty(users.Values);
            int actualUsersValueCount = users.Values.Count;
            Assert.Equal(3, actualUsersValueCount);

            KeyValuePair<Guid, UserRecord> actualUserRecord = users.FirstOrDefault(c => c.Key == userId);
            Assert.Equal(new KeyValuePair<Guid, UserRecord>().Key, actualUserRecord.Key);
            Assert.Equal(new KeyValuePair<Guid, UserRecord>().Value, actualUserRecord.Value);
            Assert.Null(actualUserRecord.Value);
        }

        [Fact]
        public async Task InMemoryUserWriteRepository_DeleteAsync_TakesNonExistingUserGuidAndDoesNotRemoveUser()
        {
            //Given
            var existingUser = new UserRecord
            {
                Id = Guid.NewGuid(),
                FirstName = "Bob",
                LastName = "Doe",
                Email = "bdoe@mail.com",
                DateOfBirth = new DateTime(1997, 12, 06),
                Age = 23
            };

            var users = new Dictionary<Guid, UserRecord>
            {
                { Guid.NewGuid(), new UserRecord() },
                { existingUser.Id, existingUser},
                { Guid.NewGuid(), new UserRecord() },
                { Guid.NewGuid(), new UserRecord() },
            };

            var usersWriteRepository = new InMemoryUserWriteRepository(users);

            //When
            await usersWriteRepository.DeleteAsync(Guid.NewGuid());

            //Then
            Assert.NotEmpty(users.Keys);
            Assert.NotEmpty(users.Values);
            int actualUsersValueCount = users.Values.Count;
            Assert.Equal(4, actualUsersValueCount);
        }
    }
}