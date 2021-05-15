namespace users_test.Users_logic_tests.Query
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Moq;
    using users_data.Entities;
    using users_data.Repositories;
    using users_logic.Exceptions.Command;
    using users_logic.Exceptions.User;
    using users_logic.User.Logic.Query;
    using users_logic.User.Logic.Query.Models.Request;
    using users_logic.User.Logic.Query.Models.Response;
    using Xunit;

    public class UserQueryGetResponseTests
    {

        public static IEnumerable<object[]> emptyRequestModels = new List<object[]>
        {
            new object[] { new GetUserRequestModel() },
            new object[] { new GetUserRequestModel { Id = Guid.Empty } }
        };

        [Fact]
        public async Task UserQuery_GetResponseAsync_TakesGetUserRequestModelAndReturnsGetUserResponseModel()
        {
            //Given
            Guid userId = Guid.NewGuid();
            var userRequestModel = new GetUserRequestModel { Id = userId };
            var existingUser = new UserRecord
            {
                Id = userId,
                FirstName = "Bob",
                LastName = "Doe",
                Email = "b.doe@email.co.uk",
                DateOfBirth = new DateTime(1992, 06, 04),
                Age = 37
            };

            var mockUserReadRepository = new Mock<IReadRepository<UserRecord>>();
            mockUserReadRepository.Setup(s => s.GetAsync(It.IsAny<Guid>())).ReturnsAsync(existingUser);

            var userQuery = new UserQuery(mockUserReadRepository.Object);

            //When
            GetUserResponseModel actualUserResponse = await userQuery.GetReponseAsync(userRequestModel);

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
        public async Task UserQuery_GetResponseAsync_TakesNullGetUserRequestModelAndThrowsArgumentNullException()
        {
            //Given
            var mockUserReadRepository = new Mock<IReadRepository<UserRecord>>();
            var userQuery = new UserQuery(mockUserReadRepository.Object);

            //When
            await Exceptions<ArgumentNullException>.HandleAsync(async () => await userQuery.GetReponseAsync(null),
                (ex) => Assert.Equal("request", ex.ParamName));

            //Then
            mockUserReadRepository.Verify(s => s.GetAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Theory]
        [MemberData(nameof(emptyRequestModels))]
        public async Task UserQuery_GetResponseAsync_TakesGetUserRequestModelAndThrowsQueryResponseException(GetUserRequestModel requestModelTest)
        {
            //Given
            var mockUserReadRepository = new Mock<IReadRepository<UserRecord>>();
            var userQuery = new UserQuery(mockUserReadRepository.Object);

            //When
            await Exceptions<QueryRequestException>.HandleAsync(async () => await userQuery.GetReponseAsync(requestModelTest),
                (ex) => Assert.Equal("Request Id cannot be empty", ex.Message));

            //Then
            mockUserReadRepository.Verify(s => s.GetAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task UserQuery_GetResponseAsync_TakesGetUserRequestModelAndThrowsUserNotFoundException()
        {
            //Given
            Guid userId = Guid.NewGuid();
            var userRequestModel = new GetUserRequestModel { Id = userId };
            var mockUserReadRepository = new Mock<IReadRepository<UserRecord>>();
            var userQuery = new UserQuery(mockUserReadRepository.Object);
            mockUserReadRepository.Setup(s => s.GetAsync(It.IsAny<Guid>())).ReturnsAsync(null as UserRecord);

            //When
            await Exceptions<UserNotFoundException>.HandleAsync(async () => await userQuery.GetReponseAsync(userRequestModel),
                (ex) => Assert.Equal("user not found", ex.Message));

            //Then
            mockUserReadRepository.Verify(s => s.GetAsync(It.IsAny<Guid>()), Times.Once);
        }
    }
}