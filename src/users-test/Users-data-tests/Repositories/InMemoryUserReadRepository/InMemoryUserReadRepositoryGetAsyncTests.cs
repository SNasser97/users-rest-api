namespace users_test.Users_data_tests.Repositories.InMemoryUserReadRepository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using users_data.Models;
    using users_data.Repositories.InMemoryUserRepository;
    using Xunit;

    public class UserReadRepositoryGetAsyncTests
    {
        [Fact]
        public async Task InMemoryUserReadRepository_GetAsync_ReturnsUserByGuid()
        {
            //Given
            var expectedUser = new UserRecord
            {
                Id = Guid.NewGuid(),
                FirstName = "Bob",
                LastName = "Doe",
                Email = "bdoe@mail.com",
                DateOfBirth = DateTime.Now,
                Age = 23
            };

            var users = new Dictionary<Guid, UserRecord>
            {
                { expectedUser.Id, expectedUser },
                { Guid.NewGuid(), new UserRecord() },
                { Guid.NewGuid(), new UserRecord() },
                { Guid.NewGuid(), new UserRecord() }
            };

            var userReadRepository = new InMemoryUserReadRepository(users);

            //When
            UserRecord actualUser = await userReadRepository.GetAsync(expectedUser.Id);

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
        }

        [Fact]
        public async Task InMemoryUserReadRepository_GetAsync_ReturnsNoUserByGuid()
        {
            //Given
            var users = new Dictionary<Guid, UserRecord>
            {
                { Guid.NewGuid(), new UserRecord() },
                { Guid.NewGuid(), new UserRecord() },
                { Guid.NewGuid(), new UserRecord() },
                { Guid.NewGuid(), new UserRecord() }
            };

            var userReadRepository = new InMemoryUserReadRepository(users);

            //When
            UserRecord actualUser = await userReadRepository.GetAsync(Guid.NewGuid());

            //Then
            Assert.Null(actualUser);
            int actualUsersInList = users.Count();
            Assert.Equal(4, actualUsersInList);
        }

        [Fact]
        public async Task InMemoryUserReadRepository_GetAsync_ReturnsNoUserByGuidOnEmptyList()
        {
            //Given
            var users = new Dictionary<Guid, UserRecord>();

            var userReadRepository = new InMemoryUserReadRepository(users);

            //When
            UserRecord actualUser = await userReadRepository.GetAsync(Guid.NewGuid());

            //Then
            Assert.Null(actualUser);
            int actualUsersInList = users.Count();
            Assert.Equal(0, actualUsersInList);
        }
    }
}