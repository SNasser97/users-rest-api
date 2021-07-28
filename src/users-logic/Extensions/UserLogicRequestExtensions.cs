namespace users_logic.Extensions
{
    using users_data.Entities;
    using users_logic.Logic.Command.CreateUserCommand;
    using users_logic.Logic.Command.UpdateUserCommand;

    public static class UserLogicRequestExtensions
    {
        public static User ToRecord(this UpdateUserCommandRequest source, int age)
        {
            return new User
            {
                Id = source.Id,
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email,
                DateOfBirth = source.DateOfBirth,
                Age = age
            };
        }

        public static User ToRecord(this CreateUserCommandRequest source, int age)
        {
            return new User
            {
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email,
                DateOfBirth = source.DateOfBirth,
                Age = age
            };
        }
    }
}