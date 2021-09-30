namespace users_test.Users_data_tests.UnitTests.Repositories.InMemoryUserReadRepository
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

    public class UserReadRepositoryGetAsyncTests
    {
        [Fact]
        public async Task InMemoryUserReadRepository_GetAsync_ReturnsUserByGuid()
        {
            //Given
            var expectedUser = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Bob",
                LastName = "Doe",
                Email = "bdoe@mail.com",
                DateOfBirth = DateTime.Now,
                Age = 23
            };

            var users = new Dictionary<Guid, User>
            {
                { expectedUser.Id, expectedUser },
                { Guid.NewGuid(), new User() },
                { Guid.NewGuid(), new User() },
                { Guid.NewGuid(), new User() }
            };

            var mockRecordData = new Mock<IRecordData<User>>();
            var userReadRepository = new InMemoryUserReadRepository(mockRecordData.Object);
            mockRecordData.SetupGet(s => s.EntityStorage).Returns(users);

            //When
            User actualUser = await userReadRepository.GetAsync(expectedUser.Id);

            //Then
            Assert.NotNull(actualUser);
            Assert.Equal(expectedUser.Id, actualUser.Id);
            Assert.Equal(expectedUser.FirstName, actualUser.FirstName);
            Assert.Equal(expectedUser.LastName, actualUser.LastName);
            Assert.Equal(expectedUser.Email, actualUser.Email);
            Assert.Equal(expectedUser.DateOfBirth, actualUser.DateOfBirth);
            Assert.Equal(expectedUser.Age, actualUser.Age);
            int actualUsersInList = users.Count();
            Assert.Equal(4, actualUsersInList);

            mockRecordData.VerifyGet(s => s.EntityStorage, Times.Once);
        }

        [Fact]
        public async Task InMemoryUserReadRepository_GetAsync_ReturnsNoUserByGuid()
        {
            //Given
            var users = new Dictionary<Guid, User>
            {
                { Guid.NewGuid(), new User() },
                { Guid.NewGuid(), new User() },
                { Guid.NewGuid(), new User() },
                { Guid.NewGuid(), new User() }
            };

            var mockRecordData = new Mock<IRecordData<User>>();
            var userReadRepository = new InMemoryUserReadRepository(mockRecordData.Object);
            mockRecordData.SetupGet(s => s.EntityStorage).Returns(users);
            //When
            User actualUser = await userReadRepository.GetAsync(Guid.NewGuid());

            //Then
            Assert.Null(actualUser);
            int actualUsersInList = users.Count();
            Assert.Equal(4, actualUsersInList);

            mockRecordData.VerifyGet(s => s.EntityStorage, Times.Once);
        }

        [Fact]
        public async Task InMemoryUserReadRepository_GetAsync_ReturnsNoUserByGuidOnEmptyList()
        {
            //Given
            var users = new Dictionary<Guid, User>();

            var mockRecordData = new Mock<IRecordData<User>>();
            var userReadRepository = new InMemoryUserReadRepository(mockRecordData.Object);
            mockRecordData.SetupGet(s => s.EntityStorage).Returns(users);

            //When
            User actualUser = await userReadRepository.GetAsync(Guid.NewGuid());

            //Then
            Assert.Null(actualUser);
            int actualUsersInList = users.Count();
            Assert.Equal(0, actualUsersInList);

            mockRecordData.VerifyGet(s => s.EntityStorage, Times.Once);
        }
    }
}