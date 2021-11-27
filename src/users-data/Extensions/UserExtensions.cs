namespace users_data.Extensions
{
    using users_data.Entities;

    public static class UserExtensions
    {
        public static void UpdateUserRecord(this User user, ref User userUpdated)
        {
            user.Firstname = userUpdated.Firstname;
            user.Lastname = userUpdated.Lastname;
            user.Email = userUpdated.Email;
            user.DateOfBirth = userUpdated.DateOfBirth;
            user.Age = userUpdated.Age;
        }
    }
}