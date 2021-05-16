namespace users_logic.Extensions
{
    using users_data.Entities;
    using users_logic.User.Logic.Command.Models.Request.Common;

    public static class UserCommandRequestExtensions
    {
        public static CreateUserRecord ToRecord(this BaseUserCommandRequest source, int age)
        {
            return new CreateUserRecord
            {
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email,
                DateOfBirth = source.DateOfBirth,
                Age = age
            };
        }

        public static UpdateUserRecord ToRecord(this BaseUserCommandRequestWithId source, int age)
        {
            return new UpdateUserRecord
            {
                Id = source.Id,
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email,
                DateOfBirth = source.DateOfBirth,
                Age = age
            };
        }
    }
}