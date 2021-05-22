namespace users_logic.Extensions
{
    using System;
    using System.Collections.Generic;
    using users_data.Entities;
    using users_logic.User.Logic.Query.Models.Response;

    public static class UserLogicResponseExtensions
    {
        public static GetUserQueryResponseModel ToResponseModel(this BaseUserRecordWithId source)
        {
            return new GetUserQueryResponseModel
            {
                Id = source.Id,
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email,
                DateOfBirth = source.DateOfBirth,
                Age = source.Age
            };
        }

        public static void ForEachResponse(this IEnumerable<GetUserQueryResponseModel> source, Action<GetUserQueryResponseModel> action)
        {
            foreach (GetUserQueryResponseModel model in source)
            {
                action(model);
            }
        }
    }
}