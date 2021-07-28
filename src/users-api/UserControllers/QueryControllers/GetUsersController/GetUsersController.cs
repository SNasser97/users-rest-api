namespace users_api.UserControllers.QueryControllers.GetUsersController
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using users_api.Extensions;
    using users_api.UserControllers.QueryControllers.Models.Response;
    using users_logic.Logic.Query;
    using users_logic.Logic.Query.GetUsersQuery;

    public class GetUsersController : BaseUserQueryController<IGetUsersQuery, GetUserQueryRequest, IEnumerable<GetUserQueryResponse>>
    {
        public GetUsersController(IGetUsersQuery query) : base(query)
        {
        }

        [HttpGet]
        public async Task<GetUserControllerResponsesModel> GetAsync()
        {
            IEnumerable<GetUserQueryResponse> queryResponses = await this.query.ExecuteAsync(null as GetUserQueryRequest);
            IEnumerable<GetUserControllerResponseModel> responses = await queryResponses.MapListToControllerResponsesAsync();
            return new GetUserControllerResponsesModel { Users = responses };
        }
    }
}