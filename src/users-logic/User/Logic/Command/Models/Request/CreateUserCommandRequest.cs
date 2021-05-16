namespace users_logic.User.Logic.Command.Models.Request
{
    using System;

    public class CreateUserCommandRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}