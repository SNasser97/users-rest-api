using System.Collections.Generic;
namespace users_test.Users_logic_tests.UnitTests.Facades
{
    using System.Threading.Tasks;
    using Moq;
    using users_logic.Facades;
    using users_logic.Parser;
    using users_logic.Provider;
    using Xunit;

    public class UserLogicFacadeIsAgeValidAsyncTests
    {
        public static IEnumerable<object[]> validAges = new List<object[]>
        {
            new object[] { 18 },
            new object[] { 110 },
            new object[] { 23 },
            new object[] { 56 },
            new object[] { 109 },
            new object[] { 19 },
            new object[] { 78 }
        };

        public static IEnumerable<object[]> invalidAges = new List<object[]>
        {
            new object[] { 17 },
            new object[] { 111 },
            new object[] { -1 },
            new object[] { 0 },
            new object[] { 1997 },
            new object[] { 10000 },
        };

        [Theory]
        [MemberData(nameof(validAges))]
        public async Task UserLogicFacade_IsAgeInvalidAsync_TakesIntegerAgeAndReturnsTrue(int validAgeTestValue)
        {
            //Given
            var mockDateTimeParser = new Mock<IDateTimeParser>();
            var mockDateTimeProvider = new Mock<IDateTimeProvider>();
            var userLogicFacade = new UserLogicFacade(mockDateTimeProvider.Object, mockDateTimeParser.Object);

            //When
            bool ageIsValid = await userLogicFacade.IsAgeValidAsync(validAgeTestValue);

            //Then
            Assert.True(ageIsValid);
        }

        [Theory]
        [MemberData(nameof(invalidAges))]
        public async Task UserLogicFacade_IsAgeInvalidAsync_TakesIntegerAgeAndReturnsFalse(int invalidAgeTestValue)
        {
            //Given
            var mockDateTimeParser = new Mock<IDateTimeParser>();
            var mockDateTimeProvider = new Mock<IDateTimeProvider>();
            var userLogicFacade = new UserLogicFacade(mockDateTimeProvider.Object, mockDateTimeParser.Object);

            //When
            bool ageIsInvalid = await userLogicFacade.IsAgeValidAsync(invalidAgeTestValue);

            //Then
            Assert.False(ageIsInvalid);
        }
    }
}