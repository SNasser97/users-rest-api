namespace users_test.Users_logic_tests.UnitTests.Query
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Moq;
    using users_data.Entities;
    using users_data.Repositories;
    using users_logic.Exceptions.Query;
    using users_logic.Exceptions.UserExceptions;
    using users_logic.Logic.Query;
    using users_logic.Logic.Query.GetUserQuery;
    using users_test.Helper;
    using Xunit;

    public class UserQueryGetResponseAsyncTests
    {

        public static IEnumerable<object[]> emptyRequestModels = new List<object[]>
        {
            new object[] { new GetUserQueryRequest() },
            new object[] { new GetUserQueryRequest { Id = Guid.Empty } }
        };

        [Fact]
        public async Task UserQuery_GetResponseAsync_TakesGetUserQueryRequestModelAndReturnsGetUserQueryResponseModel()
        {
            //Given
            Guid userId = Guid.NewGuid();
            var userRequestModel = new GetUserQueryRequest { Id = userId };
            var existingUser = new User
            {
                Id = userId,
                FirstName = "Bob",
                LastName = "Doe",
                Email = "b.doe@email.co.uk",
                DateOfBirth = new DateTime(1992, 06, 04),
                Age = 37
            };

            var mockUserReadRepository = new Mock<IReadRepository<User>>();
            mockUserReadRepository.Setup(s => s.GetAsync(It.IsAny<Guid>())).ReturnsAsync(existingUser);

            var userQuery = new GetUserQuery(mockUserReadRepository.Object);

            //When
            GetUserQueryResponse actualUserResponse = await userQuery.ExecuteAsync(userRequestModel);

            //Then
            Assert.Equal(userRequestModel.Id, actualUserResponse.Id);
            Assert.Equal(existingUser.Id, actualUserResponse.Id);
            Assert.Equal(existingUser.FirstName, actualUserResponse.FirstName);
            Assert.Equal(existingUser.LastName, actualUserResponse.LastName);
            Assert.Equal(existingUser.Email, actualUserResponse.Email);
            Assert.Equal(existingUser.DateOfBirth, actualUserResponse.DateOfBirth);
            Assert.Equal(existingUser.Age, actualUserResponse.Age);
        }

        [Fact]
        public async Task UserQuery_GetResponseAsync_TakesNullGetUserQueryRequestModelAndThrowsArgumentNullException()
        {
            //Given
            var mockUserReadRepository = new Mock<IReadRepository<User>>();
            var userQuery = new GetUserQuery(mockUserReadRepository.Object);

            //When
            await Exceptions<ArgumentNullException>.HandleAsync(async () => await userQuery.ExecuteAsync(null),
                (ex) => Assert.Equal("request", ex.ParamName));

            //Then
            mockUserReadRepository.Verify(s => s.GetAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Theory]
        [MemberData(nameof(emptyRequestModels))]
        public async Task UserQuery_GetResponseAsync_TakesGetUserRequestModelAndThrowsQueryResponseException(GetUserQueryRequest requestModelTest)
        {
            //Given
            var mockUserReadRepository = new Mock<IReadRepository<User>>();
            var userQuery = new GetUserQuery(mockUserReadRepository.Object);

            //When
            await Exceptions<QueryRequestException>.HandleAsync(async () => await userQuery.ExecuteAsync(requestModelTest),
                (ex) => Assert.Equal("Request Id was empty", ex.Message));

            //Then
            mockUserReadRepository.Verify(s => s.GetAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task UserQuery_GetResponseAsync_TakesGetUserQueryRequestModelAndThrowsUserNotFoundException()
        {
            //Given
            Guid userId = Guid.NewGuid();
            var userRequestModel = new GetUserQueryRequest { Id = userId };
            var mockUserReadRepository = new Mock<IReadRepository<User>>();
            var userQuery = new GetUserQuery(mockUserReadRepository.Object);
            mockUserReadRepository.Setup(s => s.GetAsync(It.IsAny<Guid>())).ReturnsAsync(null as User);

            //When
            await Exceptions<UserNotFoundException>.HandleAsync(async () => await userQuery.ExecuteAsync(userRequestModel),
                (ex) => Assert.Equal("User not found", ex.Message));

            //Then
            mockUserReadRepository.Verify(s => s.GetAsync(It.IsAny<Guid>()), Times.Once);
        }
    }
}