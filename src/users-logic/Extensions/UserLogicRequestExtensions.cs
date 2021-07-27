namespace users_logic.Extensions
{
    using users_data.Entities;
    using users_logic.User.Logic.Command.Models.Request.Common;

    public static class UserLogicRequestExtensions
    {
        public static User ToRecord(this BaseUserCommandRequestModel source, int age)
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

        public static User ToRecord(this BaseUserCommandRequestWithIdModel source, int age)
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
    }
}