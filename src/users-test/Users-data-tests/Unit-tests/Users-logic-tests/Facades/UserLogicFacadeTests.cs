namespace users_test.Users_logic_tests.UnitTests.Facades
{
    using System;
    using users_logic.Facades;
    using users_logic.Parser;
    using users_logic.Provider;
    using Xunit;

    public class UserLogicFacadeTests
    {
        [Fact]
        public void UserLogicFacade_TakesNullDateTimeProviderDependency_AndThrowsArgNullException()
        {
            //Given
            //When
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(()
                => new UserLogicFacade(null, new DateTimeParser()));

            //Then
            Assert.Equal("dateTimeProvider", ex.ParamName);
        }

        [Fact]
        public void UserLogicFacade_TakesNullDateTimeParserDependency_AndThrowsArgNullException()
        {
            //Given
            //When
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(()
                => new UserLogicFacade(new DateTimeProvider(), null));

            //Then
            Assert.Equal("dateTimeParser", ex.ParamName);
        }
    }
}