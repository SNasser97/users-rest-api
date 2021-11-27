namespace users_data.Entities
{
    using System;

    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Email { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int Age { get; set; }
    }
}