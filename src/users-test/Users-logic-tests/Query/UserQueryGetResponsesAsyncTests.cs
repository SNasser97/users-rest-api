namespace users_test.Users_logic_tests.Query
{
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using users_data.Entities;
    using users_data.Repositories;
    using users_logic.User.Logic.Query;
    using users_logic.User.Logic.Query.Models.Response;
    using Xunit;

    public class UserQueryGetResponsesAsyncTests
    {
        [Fact]
        public async Task UserQuery_GetResponsesAsync_ReturnsGetUsersQueryResponseModel()
        {
            //Given
            var existingUserRecords = new List<UserRecord>
            {
                new UserRecord
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Bob",
                    LastName = "Doe",
                    Email = "b.doe@email.com",
                    DateOfBirth = new DateTime(1997, 06, 12),
                    Age = 23
                },
                new UserRecord
                {
                    Id = Guid.NewGuid(),
                    FirstName = "John",
                    LastName = "Marx",
                    Email = "j.marx@yahoo.com",
                    DateOfBirth = new DateTime(2001, 11, 01),
                    Age = 19
                },
                new UserRecord
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Mary",
                    LastName = "Waltz",
                    Email = "mary_waltz@live.co.uk",
                    DateOfBirth = new DateTime(1987, 08, 28),
                    Age = 54
                }
            };

            var mockUserReadRepository = new Mock<IReadRepository<UserRecord>>();
            var userQuery = new UserQuery(mockUserReadRepository.Object);
            mockUserReadRepository.Setup(s => s.GetAsync()).ReturnsAsync(existingUserRecords);

            //When
            GetUsersQueryResponseModel actualUsersResponse = await userQuery.GetReponsesAsync();

            //Then
            Assert.NotNull(actualUsersResponse);
            Assert.NotEmpty(actualUsersResponse.Users);
            foreach (GetUserQueryResponseModel actualUserResponse in actualUsersResponse.Users)
            {
                UserRecord expectedUserMapped = existingUserRecords.FirstOrDefault(u => u.Id == actualUserResponse.Id);
                Assert.NotNull(expectedUserMapped);
                Assert.Equal(expectedUserMapped.FirstName, actualUserResponse.FirstName);
                Assert.Equal(expectedUserMapped.LastName, actualUserResponse.LastName);
                Assert.Equal(expectedUserMapped.Email, actualUserResponse.Email);
                Assert.Equal(expectedUserMapped.DateOfBirth, actualUserResponse.DateOfBirth);
                Assert.Equal(expectedUserMapped.Age, actualUserResponse.Age);
            }

            int actualUserResponseListCount = actualUsersResponse.Users.Count();
            Assert.Equal(3, actualUserResponseListCount);
            int existingUserRecordsListCount = existingUserRecords.Count();
            Assert.Equal(3, existingUserRecordsListCount);

            mockUserReadRepository.Verify(s => s.GetAsync(), Times.Once);
        }

        [Fact]
        public async Task UserQuery_GetResponsesAsync_ReturnsEmptyGetUsersQueryResponseModel()
        {
            //Given
            var mockUserReadRepository = new Mock<IReadRepository<UserRecord>>();
            var userQuery = new UserQuery(mockUserReadRepository.Object);
            mockUserReadRepository.Setup(s => s.GetAsync()).ReturnsAsync(Enumerable.Empty<UserRecord>());

            //When
            GetUsersQueryResponseModel actualUsersResponse = await userQuery.GetReponsesAsync();

            //Then
            Assert.NotNull(actualUsersResponse);
            Assert.Empty(actualUsersResponse.Users);
            int actualUserResponseListCount = actualUsersResponse.Users.Count();
            Assert.Equal(0, actualUserResponseListCount);

            mockUserReadRepository.Verify(s => s.GetAsync(), Times.Once);
        }
    }
}