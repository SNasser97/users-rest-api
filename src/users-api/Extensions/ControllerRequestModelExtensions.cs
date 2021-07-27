namespace users_api.Extensions
{
    using System;
    using System.Globalization;
    using users_api.UserControllers.CommandControllers.Models.Request.Common;
    using users_logic.Logic.Command.CreateUserCommand;
    using users_logic.Logic.Command.UpdateUserCommand;

    public static class ControllerRequestModelExtensions
    {
        public static CreateUserCommandRequest ToCommandRequest(this BaseUserControllerRequestModel source)
        {
            return new CreateUserCommandRequest
            {
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email,
                DateOfBirth = DateTime.ParseExact(source.DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture)
            };
        }

        public static UpdateUserCommandRequest ToCommandRequest(this BaseUserControllerRequestModel source, Guid routeId)
        {
            return new UpdateUserCommandRequest
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