namespace users_test.Users_data_tests.UnitTests.Repositories.InMemoryUserWriteRepository
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using users_data.Repositories.InMemoryUserRepository;
    using Xunit;
    using System.Linq;
    using users_data.Entities;
    using Moq;
    using users_data.Repositories;

    public class InMemoryUserWriteRepositoryUpdateAsyncTests
    {
        [Fact]
        public async Task InMemoryUserWriteRepository_UpdateAsync_TakesUserRecordandReturnsExistingGuid()
        {
            //Given
            var userToUpdate = new User
            {
                Id = Guid.NewGuid(),
                Firstname = "Bobby",
                Lastname = "Doenuts",
                Email = "bobby.doenuts@mail.com",
                DateOfBirth = new DateTime(2001, 12, 06),
                Age = 19
            };

            var existingUser = new User
            {
                Id = userToUpdate.Id,
                Firstname = "Bob",
                Lastname = "Doe",
                Email = "bdoe@mail.com",
                DateOfBirth = new DateTime(1997, 12, 06),
                Age = 23
            };

            var users = new Dictionary<Guid, User>
            {
                { existingUser.Id, existingUser},
                { Guid.NewGuid(), new User() }
            };

            var mockRecordData = new Mock<IRecordData<User>>();
            var usersWriteRepository = new InMemoryUserWriteRepository(mockRecordData.Object);
            mockRecordData.SetupGet(s => s.EntityStorage).Returns(users);

            //When
            Guid actualGuid = await usersWriteRepository.UpdateAsync(userToUpdate);


            //Then
            Assert.True(actualGuid != Guid.Empty);
            Assert.Equal(userToUpdate.Id, actualGuid);
            Assert.Equal(existingUser.Id, actualGuid);

            KeyValuePair<Guid, User> actualKeyValuePairUpdated = users.FirstOrDefault(u => u.Key == actualGuid);
            Assert.NotEqual(new KeyValuePair<Guid, User>(), actualKeyValuePairUpdated);
            Assert.NotNull(actualKeyValuePairUpdated.Value);

            User actualUserRecordUpdated = actualKeyValuePairUpdated.Value;
            Assert.Equal(actualUserRecordUpdated.Id, actualGuid);
            Assert.Equal("Bobby", actualUserRecordUpdated.Firstname);
            Assert.Equal("Doenuts", actualUserRecordUpdated.Lastname);
            Assert.Equal("bobby.doenuts@mail.com", actualUserRecordUpdated.Email);
            Assert.Equal(new DateTime(2001, 12, 06), actualUserRecordUpdated.DateOfBirth);
            Assert.Equal(19, actualUserRecordUpdated.Age);

            int actualUsersValueCount = users.Values.Count;
            Assert.Equal(2, actualUsersValueCount);

            mockRecordData.SetupGet(s => s.EntityStorage).Returns(users);
        }

        [Fact]
        public async Task InMemoryUserWriteRepository_UpdateAsync_TakesUserRecordReturnsEmptyGuid()
        {
            //Given
            var noneExistingUser = new User
            {
                Id = Guid.NewGuid(),
                Firstname = "Bobby",
                Lastname = "Doenuts",
                Email = "bobby.doenuts@mail.com",
                DateOfBirth = new DateTime(2001, 12, 06),
                Age = 23
            };

            var existingUser = new User
            {
                Id = Guid.NewGuid(),
                Firstname = "Bob",
                Lastname = "Doe",
                Email = "bdoe@mail.com",
                DateOfBirth = new DateTime(1997, 12, 06),
                Age = 23
            };

            var users = new Dictionary<Guid, User>
            {
                { existingUser.Id, existingUser},
                { Guid.NewGuid(), new User() }
            };

            var mockRecordData = new Mock<IRecordData<User>>();
            var usersWriteRepository = new InMemoryUserWriteRepository(mockRecordData.Object);
            mockRecordData.SetupGet(s => s.EntityStorage).Returns(users);

            //When
            Guid actualGuid = await usersWriteRepository.UpdateAsync(noneExistingUser);

            //Then
            Assert.True(actualGuid == Guid.Empty);
            Assert.Equal("Bob", existingUser.Firstname);
            Assert.Equal("Doe", existingUser.Lastname);
            Assert.Equal("bdoe@mail.com", existingUser.Email);
            Assert.Equal(new DateTime(1997, 12, 06), existingUser.DateOfBirth);
            Assert.Equal(23, existingUser.Age);

            int actualUsersValueCount = users.Values.Count;
            Assert.Equal(2, actualUsersValueCount);
        }
    }
}