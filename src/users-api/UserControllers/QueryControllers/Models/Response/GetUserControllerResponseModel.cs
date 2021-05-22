namespace users_api.UserControllers.QueryControllers.Models.Response
{
    using System;
    using users_api.UserControllers.CommandControllers.Models.Response.Common;

    public class GetUserControllerResponseModel : BaseUserControllerResponseErrorModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int Age { get; set; }
    }
}