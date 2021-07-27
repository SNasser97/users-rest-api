using System;
using users_data.Entities;

namespace users_logic.Logic.Command.CreateUserCommand
{
    public class CreateUserCommandRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int Age { get; set; }
    }
}