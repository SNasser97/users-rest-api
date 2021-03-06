namespace users_test.Users_logic_tests.UnitTests.Facades
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Moq;
    using users_data.Entities;
    using users_logic.Facades;
    using users_logic.Parser;
    using users_logic.Provider;
    using Xunit;

    public class UserLogicFacadeDoesUserEmailAlreadyExistAsyncTests
    {
        [Fact]
        public async Task UserLogicFacade_DoesUserEmailAlreadyExistAsync_ExpectsFalseOnUniqueEmail()
        {
            // Given
            string newEmail = "myunique@mail.com";

            var users = new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    Firstname = "Bob",
                    Lastname = "Doe",
                    Email = "bobo@email.com",
                    DateOfBirth = new DateTime(1997, 12, 06),
                    Age = 23
                },

                new User
                {
                    Id = Guid.NewGuid(),
                    Firstname = "jo",
                    Lastname = "marx",
                    Email = "jo.marx@email.com",
                    DateOfBirth = new DateTime(1984, 05, 21),
                    Age = 45
                },
            };

            var mockDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockDateTimeParser = new Mock<IDateTimeParser>();

            // When
            var userLogicFacade = new UserLogicFacade(mockDateTimeProvider.Object, mockDateTimeParser.Object);

            // Then
            bool actualBool = await userLogicFacade.DoesUserEmailAlreadyExistAsync(users, newEmail);

            Assert.False(actualBool);
        }

        [Fact]
        public async Task UserLogicFacade_DoesUserEmailAlreadyExistAsync_ExpectsTrueOnExistingEmail()
        {
            // Given
            string newEmail = "thisEmailExists@email.com";

            var users = new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    Firstname = "Bob",
                    Lastname = "Doe",
                    Email = "bobo@email.com",
                    DateOfBirth = new DateTime(1997, 12, 06),
                    Age = 23
                },

                new User
                {
                    Id = Guid.NewGuid(),
                    Firstname = "jo",
                    Lastname = "marx",
                    Email = "thisEmailExists@email.com",
                    DateOfBirth = new DateTime(1984, 05, 21),
                    Age = 45
                },
            };

            var mockDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockDateTimeParser = new Mock<IDateTimeParser>();

            // When
            var userLogicFacade = new UserLogicFacade(mockDateTimeProvider.Object, mockDateTimeParser.Object);

            // Then
            bool actualBool = await userLogicFacade.DoesUserEmailAlreadyExistAsync(users, newEmail);

            Assert.True(actualBool);
        }
    }
}