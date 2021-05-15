namespace users_test.Users_logic_tests.Query
{
    using System;
    using users_logic.User.Logic.Query;
    using Xunit;

    public class UserQueryTests
    {
        [Fact]
        public void UserQuery_TakesNullUserReadRepositoryDependency_ExpectsArgumentNullException()
        {
            //Given
            //When
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new UserQuery(null));

            //Then
            Assert.Equal("userReadRepository", ex.ParamName);
        }
    }
}