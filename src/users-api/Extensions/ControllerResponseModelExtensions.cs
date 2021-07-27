namespace users_api.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using users_api.UserControllers.QueryControllers.Models.Response;
    using users_logic.Logic.Query;

    public static class ControllerResponseModelExtensions
    {
        public static async Task<IEnumerable<GetUserControllerResponseModel>> MapListToControllerResponsesAsync(this IEnumerable<GetUserQueryResponse> source)
            => await Task.WhenAll(source.Select(r => Task.Run(() => r.ToControllerResponse())));

        public static GetUserControllerResponseModel ToControllerResponse(this GetUserQueryResponse source)
        {
            return new GetUserControllerResponseModel
            {
                Id = source.Id,
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email,
                DateOfBirth = source.DateOfBirth,
                Age = source.Age
            };
        }
    }
}