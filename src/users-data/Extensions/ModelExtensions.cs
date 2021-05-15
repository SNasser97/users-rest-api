namespace users_data.Extensions
{
    using users_data.Models;
    using System;

    public static class ModelExtensions
    {
        public static UserRecord ToUserRecord(this IUserRecord newUser, int validAge)
        {
            return new UserRecord
            {
                Id = Guid.NewGuid(),
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                DateOfBirth = newUser.DateOfBirth,
                Age = validAge
            };
        }
    }
}