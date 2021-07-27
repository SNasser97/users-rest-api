namespace users_data.Extensions
{
    using users_data.Entities;

    public static class ToUserRecordExtensions
    {
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