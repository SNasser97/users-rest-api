namespace users_test.Users_logic_tests.Query
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Moq;
    using users_data.Entities;
    using users_data.Repositories;
    using users_logic.Exceptions.Query;
    using users_logic.Exceptions.User;
    using users_logic.User.Logic.Query;
    using users_logic.User.Logic.Query.Models.Request;
    using users_logic.User.Logic.Query.Models.Response;
    using Xunit;

    public class UserQueryGetResponseAsyncTests
    {

        public static IEnumerable<object[]> emptyRequestModels = new List<object[]>
        {
            new object[] { new GetUserQueryRequestModel() },
            new object[] { new GetUserQueryRequestModel { Id = Guid.Empty } }
        };

        [Fact]
        public async Task UserQuery_GetResponseAsync_TakesGetUserQueryRequestModelAndReturnsGetUserQueryResponseModel()
        {
            //Given
            Guid userId = Guid.NewGuid();
            var userRequestModel = new GetUserQueryRequestModel { Id = userId };
            var existingUser = new UserRecord
            {
                Id = userId,
                FirstName = "Bob",
                LastName = "Doe",
                Email = "b.doe@email.co.uk",
                DateOfBirth = new DateTime(1992, 06, 04),
                Age = 37
            };

            var mockUserReadRepository = new Mock<IReadRepository<BaseUserRecordWithId>>();
            mockUserReadRepository.Setup(s => s.GetAsync(It.IsAny<Guid>())).ReturnsAsync(existingUser);

            var userQuery = new UserQuery(mockUserReadRepository.Object);

            //When
            GetUserQueryResponseModel actualUserResponse = await userQuery.GetResponseAsync(userRequestModel);

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
            var mockUserReadRepository = new Mock<IReadRepository<BaseUserRecordWithId>>();
            var userQuery = new UserQuery(mockUserReadRepository.Object);

            //When
            await Exceptions<ArgumentNullException>.HandleAsync(async () => await userQuery.GetResponseAsync(null),
                (ex) => Assert.Equal("request", ex.ParamName));

            //Then
            mockUserReadRepository.Verify(s => s.GetAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Theory]
        [MemberData(nameof(emptyRequestModels))]
        public async Task UserQuery_GetResponseAsync_TakesGetUserRequestModelAndThrowsQueryResponseException(GetUserQueryRequestModel requestModelTest)
        {
            //Given
            var mockUserReadRepository = new Mock<IReadRepository<BaseUserRecordWithId>>();
            var userQuery = new UserQuery(mockUserReadRepository.Object);

            //When
            await Exceptions<QueryRequestException>.HandleAsync(async () => await userQuery.GetResponseAsync(requestModelTest),
                (ex) => Assert.Equal("Request Id was empty", ex.Message));

            //Then
            mockUserReadRepository.Verify(s => s.GetAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task UserQuery_GetResponseAsync_TakesGetUserQueryRequestModelAndThrowsUserNotFoundException()
        {
            //Given
            Guid userId = Guid.NewGuid();
            var userRequestModel = new GetUserQueryRequestModel { Id = userId };
            var mockUserReadRepository = new Mock<IReadRepository<BaseUserRecordWithId>>();
            var userQuery = new UserQuery(mockUserReadRepository.Object);
            mockUserReadRepository.Setup(s => s.GetAsync(It.IsAny<Guid>())).ReturnsAsync(null as UserRecord);

            //When
            await Exceptions<UserNotFoundException>.HandleAsync(async () => await userQuery.GetResponseAsync(userRequestModel),
                (ex) => Assert.Equal("User not found", ex.Message));

            //Then
            mockUserReadRepository.Verify(s => s.GetAsync(It.IsAny<Guid>()), Times.Once);
        }
    }
}