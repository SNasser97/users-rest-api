namespace users_data.Extensions
{
    using System;
    using users_data.Entities;

    public static class ToUserRecordExtensions
    {
        public static UserRecord ToUserRecord(this BaseUserRecord record, Guid id)
        {
            return new UserRecord
            {
                Id = id,
                FirstName = record.FirstName,
                LastName = record.LastName,
                Email = record.Email,
                DateOfBirth = record.DateOfBirth,
                Age = record.Age
            };
        }

        public static void UpdateUserRecord(this BaseUserRecord user, ref BaseUserRecordWithId userUpdated)
        {
            // Will only update if Id matches
            user.FirstName = userUpdated.FirstName;
            user.LastName = userUpdated.LastName;
            user.Email = userUpdated.Email;
            user.DateOfBirth = userUpdated.DateOfBirth;
            user.Age = userUpdated.Age;
        }
    }
}