namespace users_api.Extensions
{
    using System;
    using System.Globalization;
    using users_api.UserControllers.CommandControllers.Models.Request.Common;
    using users_logic.User.Logic.Command.Models.Request;
    using users_logic.User.Logic.Command.Models.Request.Common;

    public static class ControllerRequestModelExtensions
    {
        public static BaseUserCommandRequestModel ToUserCommandRequest(this BaseUserControllerRequestModel source)
        {
            return new CreateUserCommandRequestModel
            {
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email,
                DateOfBirth = DateTime.ParseExact(source.DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture)
            };
        }

        public static BaseUserCommandRequestWithIdModel ToUserCommandRequest(this BaseUserControllerRequestModel source, Guid routeId)
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