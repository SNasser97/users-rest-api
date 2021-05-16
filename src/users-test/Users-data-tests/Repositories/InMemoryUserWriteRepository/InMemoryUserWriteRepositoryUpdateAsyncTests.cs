namespace users_test.Users_data_tests.Repositories.InMemoryUserWriteRepository
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using users_data.Repositories.InMemoryUserRepository;
    using Xunit;
    using System.Linq;
    using users_data.Entities;

    public class InMemoryUserWriteRepositoryUpdateAsyncTests
    {
        [Fact]
        public async Task InMemoryUserWriteRepository_UpdateAsync_TakesUserRecordandReturnsExistingGuid()
        {
            //Given
            var userToUpdate = new UpdateUserRecord
            {
                Id = Guid.NewGuid(),
                FirstName = "Bobby",
                LastName = "Doenuts",
                Email = "bobby.doenuts@mail.com",
                DateOfBirth = new DateTime(2001, 12, 06),
                Age = 19
            };

            var existingUser = new UserRecord
            {
                Id = userToUpdate.Id,
                FirstName = "Bob",
                LastName = "Doe",
                Email = "bdoe@mail.com",
                DateOfBirth = new DateTime(1997, 12, 06),
                Age = 23
            };

            var users = new Dictionary<Guid, UserRecord>
            {
                { existingUser.Id, existingUser},
                { Guid.NewGuid(), new UserRecord() }
            };

            var usersWriteRepository = new InMemoryUserWriteRepository(users);

            //When
            Guid actualGuid = await usersWriteRepository.UpdateAsync(userToUpdate);


            //Then
            Assert.True(actualGuid != Guid.Empty);
            Assert.Equal(userToUpdate.Id, actualGuid);
            Assert.Equal(existingUser.Id, actualGuid);

            KeyValuePair<Guid, UserRecord> actualKeyValuePairUpdated = users.FirstOrDefault(u => u.Key == actualGuid);
            Assert.NotEqual(new KeyValuePair<Guid, UserRecord>(), actualKeyValuePairUpdated);
            Assert.NotNull(actualKeyValuePairUpdated.Value);

            UserRecord actualUserRecordUpdated = actualKeyValuePairUpdated.Value;
            Assert.Equal(actualUserRecordUpdated.Id, actualGuid);
            Assert.Equal("Bobby", actualUserRecordUpdated.FirstName);
            Assert.Equal("Doenuts", actualUserRecordUpdated.LastName);
            Assert.Equal("bobby.doenuts@mail.com", actualUserRecordUpdated.Email);
            Assert.Equal(new DateTime(2001, 12, 06), actualUserRecordUpdated.DateOfBirth);
            Assert.Equal(19, actualUserRecordUpdated.Age);

            int actualUsersValueCount = users.Values.Count;
            Assert.Equal(2, actualUsersValueCount);
        }

        [Fact]
        public async Task InMemoryUserWriteRepository_UpdateAsync_TakesUserRecordReturnsEmptyGuid()
        {
            //Given
            var noneExistingUser = new UpdateUserRecord
            {
                Id = Guid.NewGuid(),
                FirstName = "Bobby",
                LastName = "Doenuts",
                Email = "bobby.doenuts@mail.com",
                DateOfBirth = new DateTime(2001, 12, 06),
                Age = 23
            };

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
                { existingUser.Id, existingUser},
                { Guid.NewGuid(), new UserRecord() }
            };

            var usersWriteRepository = new InMemoryUserWriteRepository(users);

            //When
            Guid actualGuid = await usersWriteRepository.UpdateAsync(noneExistingUser);

            //Then
            Assert.True(actualGuid == Guid.Empty);
            Assert.Equal("Bob", existingUser.FirstName);
            Assert.Equal("Doe", existingUser.LastName);
            Assert.Equal("bdoe@mail.com", existingUser.Email);
            Assert.Equal(new DateTime(1997, 12, 06), existingUser.DateOfBirth);
            Assert.Equal(23, existingUser.Age);

            int actualUsersValueCount = users.Values.Count;
            Assert.Equal(2, actualUsersValueCount);
        }
    }
}