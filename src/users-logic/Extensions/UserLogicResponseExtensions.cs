namespace users_logic.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using users_data.Entities;
    using users_logic.Logic.Query;
    using users_logic.Logic.Query.Models.Response;

    public static class UserLogicResponseExtensions
    {
        public static GetUserQueryResponse ToResponseModel(this User source)
        {
            return new GetUserQueryResponse
            {
                Id = source.Id,
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email,
                DateOfBirth = source.DateOfBirth,
                Age = source.Age
            };
        }

        public static async Task<IEnumerable<GetUserQueryResponse>> MapToGetUsersQueryResponseModel(this IEnumerable<User> source)
            => await (Task.WhenAll(source.Select(r
                => Task.Run(() => r.ToResponseModel()))));

        public static void ForEachResponse(this IEnumerable<GetUserQueryResponseModel> source, Action<GetUserQueryResponseModel> action)
        {
            foreach (GetUserQueryResponseModel model in source)
            {
                action(model);
            }
        }
    }
}