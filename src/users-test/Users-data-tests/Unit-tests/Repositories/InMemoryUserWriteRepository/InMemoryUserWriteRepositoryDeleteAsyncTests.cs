namespace users_test.Users_data_tests.UnitTests.Repositories.InMemoryUserWriteRepository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Moq;
    using users_data.Entities;
    using users_data.Repositories;
    using users_data.Repositories.InMemoryUserRepository;
    using Xunit;

    public class InMemoryUserWriteRepositoryDeleteAsyncTests
    {
        [Fact]
        public async Task InMemoryUserWriteRepository_DeleteAsync_TakesExistingUserGuidAndRemovesUser()
        {
            //Given
            Guid userId = Guid.NewGuid();

            var existingUser = new User
            {
                Id = userId,
                FirstName = "Bob",
                LastName = "Doe",
                Email = "bdoe@mail.com",
                DateOfBirth = new DateTime(1997, 12, 06),
                Age = 23
            };

            var users = new Dictionary<Guid, User>
            {
                { Guid.NewGuid(), new User() },
                { existingUser.Id, existingUser},
                { Guid.NewGuid(), new User() },
                { Guid.NewGuid(), new User() },
            };

            var mockRecordData = new Mock<IRecordData<User>>();
            var usersWriteRepository = new InMemoryUserWriteRepository(mockRecordData.Object);
            mockRecordData.SetupGet(s => s.EntityStorage).Returns(users);

            //When
            await usersWriteRepository.DeleteAsync(userId);

            //Then
            Assert.NotEmpty(users.Keys);
            Assert.NotEmpty(users.Values);
            int actualUsersValueCount = users.Values.Count;
            Assert.Equal(3, actualUsersValueCount);

            KeyValuePair<Guid, User> actualUserRecord = users.FirstOrDefault(c => c.Key == userId);
            Assert.Equal(new KeyValuePair<Guid, User>().Key, actualUserRecord.Key);
            Assert.Equal(new KeyValuePair<Guid, User>().Value, actualUserRecord.Value);
            Assert.Null(actualUserRecord.Value);
            mockRecordData.VerifyGet(s => s.EntityStorage, Times.Once);
        }

        [Fact]
        public async Task InMemoryUserWriteRepository_DeleteAsync_TakesNonExistingUserGuidAndDoesNotRemoveUser()
        {
            //Given
            var existingUser = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Bob",
                LastName = "Doe",
                Email = "bdoe@mail.com",
                DateOfBirth = new DateTime(1997, 12, 06),
                Age = 23
            };

            var users = new Dictionary<Guid, User>
            {
                { Guid.NewGuid(), new User() },
                { existingUser.Id, existingUser},
                { Guid.NewGuid(), new User() },
                { Guid.NewGuid(), new User() },
            };

            var mockRecordData = new Mock<IRecordData<User>>();
            var usersWriteRepository = new InMemoryUserWriteRepository(mockRecordData.Object);
            mockRecordData.SetupGet(s => s.EntityStorage).Returns(users);

            //When
            await usersWriteRepository.DeleteAsync(Guid.NewGuid());

            //Then
            Assert.NotEmpty(users.Keys);
            Assert.NotEmpty(users.Values);
            int actualUsersValueCount = users.Values.Count;
            Assert.Equal(4, actualUsersValueCount);

            mockRecordData.VerifyGet(s => s.EntityStorage, Times.Once);
        }
    }
}