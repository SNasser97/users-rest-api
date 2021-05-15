namespace users_test.Users_data_tests.Facades
{
    using System;
    using users_data.Facades;
    using users_data.Repositories.InMemoryUserRepository.Data.Parser;
    using users_data.Repositories.InMemoryUserRepository.Data.Provider;
    using users_data.Repositories.InMemoryUserRepository.Data.Validation;
    using Xunit;

    public class UserFacadeTests
    {
        [Fact]
        public void UserFacade_TakesNullUserRecordValidatorDependency_AndThrowsArgNullException()
        {
            //Given
            //When
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(()
                => new UserFacade(null, new DateTimeProvider(), new DateTimeParser()));

            //Then
            Assert.Equal("verifyRecord", ex.ParamName);
        }

        [Fact]
        public void UserFacade_TakesNullDateTimeProviderDependency_AndThrowsArgNullException()
        {
            //Given
            //When
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(()
                => new UserFacade(new UserRecordValidator(), null, new DateTimeParser()));

            //Then
            Assert.Equal("dateTimeProvider", ex.ParamName);
        }

        [Fact]
        public void UserFacade_TakesNullDateTimeParserDependency_AndThrowsArgNullException()
        {
            //Given
            //When
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(()
                => new UserFacade(new UserRecordValidator(), new DateTimeProvider(), null));

            //Then
            Assert.Equal("dateTimeParser", ex.ParamName);
        }
    }
}