namespace users_test.Users_data_tests.Repositories.InMemoryUserReadRepository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using users_data.Entities;
    using users_data.Repositories.InMemoryUserRepository;
    using Xunit;

    public class InMemoryUserReadRepositoryGetAsyncUserListTests
    {
        [Fact]
        public async Task InMemoryUserReadRepository_GetAsync_ReturnsListOfUsers()
        {
            //Given
            Guid userOne = Guid.NewGuid();
            Guid userTwo = Guid.NewGuid();
            Guid userThree = Guid.NewGuid();

            var expectedUsersData = new Dictionary<Guid, UserRecord>
            {
                { userOne , new UserRecord { Id = userOne, FirstName = "Bob", LastName = "Doe", Email = "b.doe@hotmail.co.uk", DateOfBirth = DateTime.Now, Age = 21 } },
                { userTwo, new UserRecord { Id = userTwo, FirstName = "Tony", LastName = "Slark", Email = "t.slark@yahoomail.com", DateOfBirth = DateTime.Now, Age = 45 } },
                { userThree, new UserRecord { Id = userThree, FirstName = "Mary", LastName = "Poppins", Email = "m.poppins@outlook.com", DateOfBirth = DateTime.Now, Age = 24 } }
            };

            var userReadRepository = new InMemoryUserReadRepository(expectedUsersData);

            //When
            IEnumerable<BaseUserRecordWithId> actualUsers = await userReadRepository.GetAsync();

            //Then
            Assert.NotNull(actualUsers);
            Assert.NotEmpty(actualUsers);
            int actualUsersInList = actualUsers.Count();
            Assert.Equal(3, actualUsersInList);

            foreach (UserRecord actualUser in actualUsers)
            {
                KeyValuePair<Guid, UserRecord> expected = expectedUsersData.FirstOrDefault(u => u.Key == actualUser.Id);
                Assert.NotEqual(new KeyValuePair<Guid, UserRecord>(), expected);

                UserRecord expectedUser = expected.Value;
                Assert.NotNull(expected.Value);
                Assert.Equal(expectedUser.Id, actualUser.Id);
                Assert.Equal(expectedUser.FirstName, actualUser.FirstName);
                Assert.Equal(expectedUser.LastName, actualUser.LastName);
                Assert.Equal(expectedUser.Email, actualUser.Email);
                Assert.Equal(expectedUser.DateOfBirth, actualUser.DateOfBirth);
                Assert.Equal(expectedUser.Age, actualUser.Age);
            }
        }

        [Fact]
        public async Task InMemoryUserReadRepository_GetAsync_ReturnsEmptyUsersList()
        {
            //Given
            var expectedUsersData = new Dictionary<Guid, UserRecord>();

            var userReadRepository = new InMemoryUserReadRepository(expectedUsersData);

            //When
            IEnumerable<BaseUserRecordWithId> actualUsers = await userReadRepository.GetAsync();

            //Then
            Assert.Empty(actualUsers);
            int actualUsersInList = actualUsers.Count();
            Assert.Equal(0, actualUsersInList);
        }
    }
}