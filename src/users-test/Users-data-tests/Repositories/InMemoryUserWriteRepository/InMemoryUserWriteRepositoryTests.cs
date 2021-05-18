namespace users_test.Users_data_tests.Repositories.InMemoryUserWriteRepository
{
    using System;
    using users_data.Repositories.InMemoryUserRepository;
    using Xunit;

    public class InMemoryUserWriteRepositoryTests
    {
        [Fact]
        public void InMemoryUserWriteRepository_TakesNullRecordDataDependency_AndThrowsArgumentNullException()
        {
            // Given
            // When
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new InMemoryUserWriteRepository(null));

            // Then
            Assert.Equal("recordData", ex.ParamName);
        }
    }
}