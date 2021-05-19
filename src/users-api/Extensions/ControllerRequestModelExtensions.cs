namespace users_api.Extensions
{
    using System;
    using System.Globalization;
    using users_api.UserControllers.CommandControllers.Models.Request.Common;
    using users_logic.User.Logic.Command.Models.Request;

    public static class ControllerRequestModelExtensions
    {
        public static CreateUserCommandRequestModel ToUserCommandRequest(this BaseUserControllerRequestModel source)
        {
            return new CreateUserCommandRequestModel
            {
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email,
                DateOfBirth = DateTime.ParseExact(source.DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture)
            };
        }

        public static UpdateUserCommandRequestModel ToUserCommandRequest(this BaseUserControllerRequestModelWithId source, Guid routeId)
        {
            return new UpdateUserCommandRequestModel
            {
                Id = routeId,
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email,
                DateOfBirth = DateTime.ParseExact(source.DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture)
            };
        }
    }
}