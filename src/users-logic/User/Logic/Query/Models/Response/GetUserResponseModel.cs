namespace users_logic.User.Logic.Query.Models.Response
{
    using System;
    using users_data.Entities;

    public class GetUserResponseModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int Age { get; set; }

        public GetUserResponseModel ToUserResponseModel(UserRecord userRecord)
        {
            return new GetUserResponseModel
            {
                Id = userRecord.Id,
                FirstName = userRecord.FirstName,
                LastName = userRecord.LastName,
                Email = userRecord.Email,
                DateOfBirth = userRecord.DateOfBirth,
                Age = userRecord.Age
            };
        }
    }
}