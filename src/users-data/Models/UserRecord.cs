namespace users_data.Models
{
    using System;

    public class UserRecord : BaseRecord, IUserRecord
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int Age { get; set; }
    }
}
