namespace users_test.Users_data_tests.Repositories.InMemoryUserWriteRepository
{
    using System;
    using users_data.Repositories.InMemoryUserRepository;
    using Xunit;

    public class InMemoryUserWriteRepositoryTests
    {
        [Fact]
        public void InMemoryUserWriteRepository_TakesNullUsersDependency_AndThrowsArgumentNullException()
        {
            // Given
            // When
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new InMemoryUserWriteRepository(null));

            // Then
            Assert.Equal("users", ex.ParamName);
        }
    }
}