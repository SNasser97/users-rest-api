namespace users_test.Users_data_tests.Repositories.InMemoryUserReadRepository
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

    public class InMemoryUserReadRepositoryGetAsyncUserListTests
    {
        [Fact]
        public async Task InMemoryUserReadRepository_GetAsync_ReturnsListOfUsers()
        {
            //Given
            Guid userOne = Guid.NewGuid();
            Guid userTwo = Guid.NewGuid();
            Guid userThree = Guid.NewGuid();

            var expectedUsersData = new Dictionary<Guid, User>
            {
                { userOne , new User { Id = userOne, FirstName = "Bob", LastName = "Doe", Email = "b.doe@hotmail.co.uk", DateOfBirth = DateTime.Now, Age = 21 } },
                { userTwo, new User { Id = userTwo, FirstName = "Tony", LastName = "Slark", Email = "t.slark@yahoomail.com", DateOfBirth = DateTime.Now, Age = 45 } },
                { userThree, new User { Id = userThree, FirstName = "Mary", LastName = "Poppins", Email = "m.poppins@outlook.com", DateOfBirth = DateTime.Now, Age = 24 } }
            };

            var mockRecordData = new Mock<IRecordData<User>>();
            var userReadRepository = new InMemoryUserReadRepository(mockRecordData.Object);
            mockRecordData.SetupGet(s => s.EntityStorage).Returns(expectedUsersData);

            //When
            IEnumerable<User> actualUsers = await userReadRepository.GetAsync();

            //Then
            Assert.NotNull(actualUsers);
            Assert.NotEmpty(actualUsers);
            int actualUsersInList = actualUsers.Count();
            Assert.Equal(3, actualUsersInList);

            foreach (User actualUser in actualUsers)
            {
                KeyValuePair<Guid, User> expected = expectedUsersData.FirstOrDefault(u => u.Key == actualUser.Id);
                Assert.NotEqual(new KeyValuePair<Guid, User>(), expected);

                User expectedUser = expected.Value;
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
            var expectedUsersData = new Dictionary<Guid, User>();

            var mockRecordData = new Mock<IRecordData<User>>();
            var userReadRepository = new InMemoryUserReadRepository(mockRecordData.Object);
            mockRecordData.SetupGet(s => s.EntityStorage).Returns(expectedUsersData);

            //When
            IEnumerable<User> actualUsers = await userReadRepository.GetAsync();

            //Then
            Assert.Empty(actualUsers);
            int actualUsersInList = actualUsers.Count();
            Assert.Equal(0, actualUsersInList);

            mockRecordData.VerifyGet(s => s.EntityStorage, Times.Once);
        }
    }
}