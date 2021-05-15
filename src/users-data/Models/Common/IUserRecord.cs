namespace users_data.Models
{
    using System;

    public interface IUserRecord
    {
        string FirstName { get; set; }

        string LastName { get; set; }

        string Email { get; set; }

        DateTime DateOfBirth { get; set; }
    }
}