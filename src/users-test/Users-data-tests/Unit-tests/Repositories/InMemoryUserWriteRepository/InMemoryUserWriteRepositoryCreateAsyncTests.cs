namespace users_test.Users_data_tests.UnitTests.Repositories.InMemoryUserWriteRepository
{
    using System.Linq;
    using System.Threading.Tasks;
    using users_data.Repositories.InMemoryUserRepository;
    using Xunit;
    using System.Collections.Generic;
    using System;
    using users_data.Entities;
    using users_data.Repositories;
    using Moq;

    public class InMemoryUserWriteRepositoryCreateAsyncTests
    {
        [Fact]
        public async Task InMemoryUserWriteRepository_CreateAsync_ReturnsAGuid()
        {
            //Given
            var newUser = new User
            {
                Firstname = "Bob",
                Lastname = "Doe",
                Email = "bdoe@mail.com",
                DateOfBirth = new DateTime(1997, 12, 06),
                Age = 24
            };

            var users = new Dictionary<Guid, User>
            {
                { Guid.NewGuid(), new User() },
                { Guid.NewGuid(), new User() },
                { Guid.NewGuid(), new User() },
            };

            var mockRecordData = new Mock<IRecordData<User>>();
            mockRecordData.SetupGet(s => s.EntityStorage).Returns(users);

            var usersWriteRepository = new InMemoryUserWriteRepository(mockRecordData.Object);
            //When
            Guid actualGuid = await usersWriteRepository.CreateAsync(newUser);

            //Then
            Assert.True(actualGuid != Guid.Empty);
            Assert.Equal(4, users.Count());

            KeyValuePair<Guid, User> actualUserKeyValuePair = users.FirstOrDefault(u => u.Key == actualGuid);
            Assert.NotEqual(default(KeyValuePair<Guid, User>), actualUserKeyValuePair);
            Assert.True(actualUserKeyValuePair.Key != Guid.Empty);
            Assert.Equal(actualUserKeyValuePair.Key, actualGuid);

            User actualNewUser = actualUserKeyValuePair.Value;
            Assert.NotNull(actualUserKeyValuePair.Value);
            Assert.Equal(newUser.Firstname, actualNewUser.Firstname);
            Assert.Equal(newUser.Lastname, actualNewUser.Lastname);
            Assert.Equal(newUser.Email, actualNewUser.Email);
            Assert.Equal(newUser.DateOfBirth, actualNewUser.DateOfBirth);
            Assert.Equal(newUser.Age, actualNewUser.Age);

            mockRecordData.VerifyGet(s => s.EntityStorage, Times.Once);
        }
    }
}