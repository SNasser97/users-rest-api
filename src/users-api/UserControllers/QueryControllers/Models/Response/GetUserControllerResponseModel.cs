namespace users_api.UserControllers.QueryControllers.Models.Response
{
    using System;

    public class GetUserControllerResponseModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string DateOfBirth { get; set; }

        public int Age { get; set; }
    }
}