namespace users_test.Users_logic_tests.Command
{
    using System;
    using users_data.Repositories.InMemoryUserRepository;
    using users_logic.User.Logic.Command;
    using Xunit;

    public class UserCommandTests
    {
        [Fact]
        public void UserCommand_TakesNullUserWriteRepositoryDependency_ExpectsArgumentNullException()
        {
            //Given
            //When
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new UserCommand(null, null, null));

            //Then
            Assert.Equal("userWriteRepository", ex.ParamName);
        }

        [Fact]
        public void UserCommand_TakesNullUserReadRepositoryDependency_ExpectsArgumentNullException()
        {
            //Given
            //When
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() =>
                new UserCommand(new InMemoryUserWriteRepository(), null, null));

            //Then
            Assert.Equal("userReadRepository", ex.ParamName);
        }

        [Fact]
        public void UserCommand_TakesNullUserLogicFacadeDependency_ExpectsArgumentNullException()
        {
            //Given
            //When
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() =>
                new UserCommand(new InMemoryUserWriteRepository(), new InMemoryUserReadRepository(), null));

            //Then
            Assert.Equal("userLogicFacade", ex.ParamName);
        }
    }
}