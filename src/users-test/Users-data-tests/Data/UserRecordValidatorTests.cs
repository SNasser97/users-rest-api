namespace users_test.Users_data_tests.Data
{
    using System;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Linq;
    using users_data.Models;
    using users_data.Repositories.InMemoryUserRepository.Data.Validation;
    using Xunit;

    public class UserRecordValidatorTests
    {
        // Test data
        public static IEnumerable<UserRecord> usersWithExistingEmail = new List<UserRecord>
            {
                new UserRecord
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Liz",
                    LastName = "Jo",
                    Email = "b.doe@mail.com",
                    DateOfBirth = new DateTime(1994, 04, 12)
                },

                new UserRecord
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Alex",
                    LastName = "Su",
                    Email = "alex.su@mail.com",
                    DateOfBirth = new DateTime(2001, 06, 03)
                },
            };

        public static IEnumerable<UserRecord> usersWithNoExistingEmail = new List<UserRecord>
            {
                new UserRecord
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Liz",
                    LastName = "Jo",
                    Email = "liz.joe@mail.com",
                    DateOfBirth = new DateTime(1994, 04, 12)
                },

                new UserRecord
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Alex",
                    LastName = "Su",
                    Email = "alex.su@mail.com",
                    DateOfBirth = new DateTime(2001, 06, 03)
                },
            };

        [Fact]
        public async Task UserRecordValidator_DoesEmailExistAsyncOnEmptyUsers_ExpectsFalseOnExistingEmail()
        {
            //Given
            var userRecordValidator = new UserRecordValidator();

            var user = new CreateUserRecord
            {
                FirstName = "Bob",
                LastName = "Doe",
                Email = "b.doe@mail.com",
                DateOfBirth = new DateTime(1997, 12, 06)
            };

            //When
            bool actualResult = await userRecordValidator
                .DoesEmailExistAsync(Enumerable.Empty<UserRecord>(), user.Email);

            //Then
            Assert.False(actualResult);
        }

        [Fact]
        public async Task UserRecordValidator_DoesEmailExistAsyncOnUsers_ExpectsFalseOnExistingEmail()
        {
            //Given
            var userRecordValidator = new UserRecordValidator();

            var user = new CreateUserRecord
            {
                FirstName = "Bob",
                LastName = "Doe",
                Email = "b.doe@mail.com",
                DateOfBirth = new DateTime(1997, 12, 06)
            };

            //When
            bool actualResult = await userRecordValidator
                .DoesEmailExistAsync(usersWithNoExistingEmail, user.Email);

            //Then
            Assert.False(actualResult);
        }

        [Fact]
        public async Task UserRecordValidator_DoesEmailExistAsyncOnUsers_ExpectsTrueOnExistingEmail()
        {
            //Given
            var userRecordValidator = new UserRecordValidator();

            var user = new CreateUserRecord
            {
                FirstName = "Bob",
                LastName = "Doe",
                Email = "b.doe@mail.com",
                DateOfBirth = new DateTime(1997, 12, 06)
            };

            //When
            bool actualResult = await userRecordValidator
                .DoesEmailExistAsync(usersWithExistingEmail, user.Email);

            //Then
            Assert.True(actualResult);
        }
    }
}