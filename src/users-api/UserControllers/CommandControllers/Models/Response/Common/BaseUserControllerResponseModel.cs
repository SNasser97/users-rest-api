namespace users_api.UserControllers.CommandControllers.Models.Response.Common
{
    using System;

    public abstract class BaseUserControllerResponseModel
    {
        public Guid Id { get; set; }

        public string? Error { get; set; }
    }
}