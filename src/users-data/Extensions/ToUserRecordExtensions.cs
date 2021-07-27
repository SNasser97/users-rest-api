namespace users_data.Extensions
{
    using System;
    using users_data.Entities;

    public static class ToUserRecordExtensions
    {
        public static User ToUserRecord(this User record, Guid id)
        {
            return new User
            {
                Id = id,
                FirstName = record.FirstName,
                LastName = record.LastName,
                Email = record.Email,
                DateOfBirth = record.DateOfBirth,
                Age = record.Age
            };
        }

        public static void UpdateUserRecord(this User user, ref User userUpdated)
        {
            user.FirstName = userUpdated.FirstName;
            user.LastName = userUpdated.LastName;
            user.Email = userUpdated.Email;
            user.DateOfBirth = userUpdated.DateOfBirth;
            user.Age = userUpdated.Age;
        }
    }
}