namespace users_test.Users_data_tests.Repositories.InMemoryUserReadRepository
{
    using System;
    using Xunit;
    using users_data.Repositories.InMemoryUserRepository;

    public class InMemoryUserReadRepositoryTests
    {
        [Fact]
        public void InMemoryUserReadRepository_TakesNullDependency_AndThrowsArgumentNullException()
        {
            // Given
            // When
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new InMemoryUserReadRepository(null));

            // Then
            Assert.Equal("recordData", ex.ParamName);
        }
    }
}
