namespace users_test.Users_data_tests.Facades
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Moq;
    using users_data.Facades;
    using users_data.Models;
    using users_data.Repositories.InMemoryUserRepository.Data.Parser;
    using users_data.Repositories.InMemoryUserRepository.Data.Provider;
    using users_data.Repositories.InMemoryUserRepository.Data.Validation;
    using Xunit;

    public class UserFacadeTestsCanUserBeInsertedTests
    {
        [Fact]
        public async Task UserFacade_CanUserBeInsertedAsync_ExpectsTrueWhenAgeIsValidButEmailDoesNotExist()
        {
            //Given
            var mockDateTimeProvider = new Mock<IDateTimeProvider>();
            mockDateTimeProvider.SetupGet(s => s.Now).Returns(DateTime.Now);

            var mockDateTimeParser = new Mock<IDateTimeParser>();
            mockDateTimeParser.Setup(s => s.ParseDateTimeAsAgeAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(23);

            var mockUserRecordValidator = new Mock<IVerifyRecord<UserRecord>>();
            mockUserRecordValidator.Setup(s => s.IsUserRecordValidAgeAsync(It.IsAny<int>())).ReturnsAsync(true);
            mockUserRecordValidator.Setup(s => s.DoesEmailExistAsync(It.IsAny<IEnumerable<UserRecord>>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var userFacade = new UserFacade(mockUserRecordValidator.Object, mockDateTimeProvider.Object, mockDateTimeParser.Object);

            //When
            bool actualResult = await userFacade.CanUserBeInsertedAsync(new List<UserRecord>(), new CreateUserRecord());

            //Then
            Assert.True(actualResult);
        }

        [Fact]
        public async Task UserFacade_CanUserBeInsertedAsync_ExpectsFalseWhenAgeIsInvalidButEmailExists()
        {
            //Given
            var mockDateTimeProvider = new Mock<IDateTimeProvider>();
            mockDateTimeProvider.SetupGet(s => s.Now).Returns(DateTime.Now);

            var mockDateTimeParser = new Mock<IDateTimeParser>();
            mockDateTimeParser.Setup(s => s.ParseDateTimeAsAgeAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(15);

            var mockUserRecordValidator = new Mock<IVerifyRecord<UserRecord>>();
            mockUserRecordValidator.Setup(s => s.IsUserRecordValidAgeAsync(It.IsAny<int>())).ReturnsAsync(false);
            mockUserRecordValidator.Setup(s => s.DoesEmailExistAsync(It.IsAny<IEnumerable<UserRecord>>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var userFacade = new UserFacade(mockUserRecordValidator.Object, mockDateTimeProvider.Object, mockDateTimeParser.Object);

            //When
            bool actualResult = await userFacade.CanUserBeInsertedAsync(new List<UserRecord>(), new CreateUserRecord());

            //Then
            Assert.False(actualResult);
        }

        [Fact]
        public async Task UserFacade_CanUserBeInsertedAsync_ExpectsFalseWhenAgeIsValidButEmailExists()
        {
            //Given
            var mockDateTimeProvider = new Mock<IDateTimeProvider>();
            mockDateTimeProvider.SetupGet(s => s.Now).Returns(DateTime.Now);

            var mockDateTimeParser = new Mock<IDateTimeParser>();
            mockDateTimeParser.Setup(s => s.ParseDateTimeAsAgeAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(24);

            var mockUserRecordValidator = new Mock<IVerifyRecord<UserRecord>>();
            mockUserRecordValidator.Setup(s => s.IsUserRecordValidAgeAsync(It.IsAny<int>())).ReturnsAsync(true);
            mockUserRecordValidator.Setup(s => s.DoesEmailExistAsync(It.IsAny<IEnumerable<UserRecord>>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var userFacade = new UserFacade(mockUserRecordValidator.Object, mockDateTimeProvider.Object, mockDateTimeParser.Object);

            //When
            bool actualResult = await userFacade.CanUserBeInsertedAsync(new List<UserRecord>(), new CreateUserRecord());

            //Then
            Assert.False(actualResult);
        }
    }
}