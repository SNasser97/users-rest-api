namespace users_api.UserControllers.CommandControllers.Models.Response.Common
{
    using System;

    public abstract class BaseUserControllerResponseModel : BaseUserControllerResponseErrorModel
    {
        public Guid Id { get; set; }
    }
}