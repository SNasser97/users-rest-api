namespace users_test.Users_data_tests.Repositories.InMemoryUserWriteRepository
{
    using System;
    using System.Collections.Generic;
    using users_data.Models;
    using users_data.Repositories.InMemoryUserRepository;
    using Xunit;

    public class InMemoryUserWriteRepositoryTests
    {
        [Fact]
        public void InMemoryUserWriteRepository_TakesNullUsersDependency_AndThrowsArgumentNullException()
        {
            // Given
            // When
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new InMemoryUserWriteRepository(null, null));

            // Then
            Assert.Equal("users", ex.ParamName);
        }

        [Fact]
        public void InMemoryUserWriteRepository_TakesNullUserFacadeDependency_AndThrowsArgumentNullException()
        {
            // Given
            // When
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new InMemoryUserWriteRepository(new Dictionary<Guid, UserRecord>(), null));

            // Then
            Assert.Equal("userFacade", ex.ParamName);
        }
    }
}