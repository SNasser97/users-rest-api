namespace users_test.Users_logic_tests.UnitTests.Query
{
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using users_data.Entities;
    using users_data.Repositories;
    using users_logic.Logic.Query;
    using users_logic.Logic.Query.GetUsersQuery;
    using Xunit;

    public class UserQueryGetResponsesAsyncTests
    {
        [Fact]
        public async Task UserQuery_GetResponsesAsync_ReturnsGetUsersQueryResponseModel()
        {
            //Given
            var existingUserRecords = new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    Firstname = "Bob",
                    Lastname = "Doe",
                    Email = "b.doe@email.com",
                    DateOfBirth = new DateTime(1997, 06, 12),
                    Age = 23
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Firstname = "John",
                    Lastname = "Marx",
                    Email = "j.marx@yahoo.com",
                    DateOfBirth = new DateTime(2001, 11, 01),
                    Age = 19
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Firstname = "Mary",
                    Lastname = "Waltz",
                    Email = "mary_waltz@live.co.uk",
                    DateOfBirth = new DateTime(1987, 08, 28),
                    Age = 54
                }
            };

            var mockUserReadRepository = new Mock<IReadRepository<User>>();
            var userQuery = new GetUsersQuery(mockUserReadRepository.Object);
            mockUserReadRepository.Setup(s => s.GetAsync()).ReturnsAsync(existingUserRecords);

            //When
            IEnumerable<GetUserQueryResponse> actualUsersResponse = await userQuery.ExecuteAsync(null as GetUserQueryRequest);

            //Then
            Assert.NotNull(actualUsersResponse);
            Assert.NotEmpty(actualUsersResponse);

            foreach (GetUserQueryResponse actualUserResponse in actualUsersResponse)
            {
                User expectedUserMapped = existingUserRecords.FirstOrDefault(u => u.Id == actualUserResponse.Id);
                Assert.NotNull(expectedUserMapped);
                Assert.Equal(expectedUserMapped.Firstname, actualUserResponse.FirstName);
                Assert.Equal(expectedUserMapped.Lastname, actualUserResponse.LastName);
                Assert.Equal(expectedUserMapped.Email, actualUserResponse.Email);
                Assert.Equal(expectedUserMapped.DateOfBirth, actualUserResponse.DateOfBirth);
                Assert.Equal(expectedUserMapped.Age, actualUserResponse.Age);
            }

            int actualUserResponseListCount = actualUsersResponse.Count();
            Assert.Equal(3, actualUserResponseListCount);
            int existingUserRecordsListCount = existingUserRecords.Count();
            Assert.Equal(3, existingUserRecordsListCount);

            mockUserReadRepository.Verify(s => s.GetAsync(), Times.Once);
        }

        [Fact]
        public async Task UserQuery_GetResponsesAsync_ReturnsEmptyGetUsersQueryResponseModel()
        {
            //Given
            var mockUserReadRepository = new Mock<IReadRepository<User>>();
            var userQuery = new GetUsersQuery(mockUserReadRepository.Object);
            mockUserReadRepository.Setup(s => s.GetAsync()).ReturnsAsync(Enumerable.Empty<User>());

            //When
            IEnumerable<GetUserQueryResponse> actualUsersResponse = await userQuery.ExecuteAsync(null as GetUserQueryRequest);

            //Then
            Assert.NotNull(actualUsersResponse);
            Assert.Empty(actualUsersResponse);
            int actualUserResponseListCount = actualUsersResponse.Count();
            Assert.Equal(0, actualUserResponseListCount);

            mockUserReadRepository.Verify(s => s.GetAsync(), Times.Once);
        }
    }
}