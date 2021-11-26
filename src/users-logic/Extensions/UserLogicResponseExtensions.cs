namespace users_logic.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using users_data.Entities;
    using users_logic.Logic.Query;

    public static class UserLogicResponseExtensions
    {
        public static GetUserQueryResponse ToResponseModel(this User source)
        {
            return new GetUserQueryResponse
            {
                Id = source.Id,
                FirstName = source.Firstname,
                LastName = source.Lastname,
                Email = source.Email,
                DateOfBirth = source.DateOfBirth,
                Age = source.Age
            };
        }

        public static async Task<IEnumerable<GetUserQueryResponse>> MapToGetUsersQueryResponseModel(this IEnumerable<User> source)
            => await (Task.WhenAll(source.Select(r
                => Task.Run(() => r.ToResponseModel()))));
    }
}