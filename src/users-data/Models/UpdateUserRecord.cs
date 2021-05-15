namespace users_data.Models
{
    using System;

    public class UpdateUserRecord : BaseRecord, IUserRecord
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}