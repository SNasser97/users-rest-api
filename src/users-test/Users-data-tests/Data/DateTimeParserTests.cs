namespace users_test.Users_data_tests.Data
{
    using System.Threading.Tasks;
    using users_data.Repositories.InMemoryUserRepository.Data.Parser;
    using Xunit;
    using System;
    using Moq;
    using users_data.Repositories.InMemoryUserRepository.Data.Provider;

    public class DateTimeParserTests
    {
        [Fact]
        public async Task DateTimeParser_TakesDateOfBirth_ExpectsTwentyFour()
        {
            //Given
            DateTime currentDate = DateTime.Now;
            var mockDateTimeProvider = new Mock<IDateTimeProvider>();
            mockDateTimeProvider.SetupGet(s => s.Now).Returns(currentDate);

            var dateTimeParser = new DateTimeParser();

            DateTime dob = new DateTime(1997, 12, 06);

            //When
            int actualAge = await dateTimeParser.ParseDateTimeAsAgeAsync(currentDate, dob);

            //Then
            Assert.Equal(24, actualAge);
        }

        [Fact]
        public async Task DateTimeParser_TakesDateOfBirth_ExpectsZero()
        {
            //Given
            DateTime currentDate = DateTime.Now;
            var mockDateTimeProvider = new Mock<IDateTimeProvider>();
            mockDateTimeProvider.SetupGet(s => s.Now).Returns(currentDate);

            var dateTimeParser = new DateTimeParser();

            DateTime dob = new DateTime(2021, 05, 13);

            //When
            int actualAge = await dateTimeParser.ParseDateTimeAsAgeAsync(currentDate, dob);

            //Then
            Assert.Equal(0, actualAge);
        }
    }
}