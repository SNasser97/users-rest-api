namespace users_api.UserControllers.QueryControllers.GetUserController
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using users_api.Extensions;
    using users_api.UserControllers.QueryControllers.Models.Response;
    using users_logic.Logic.Query;
    using users_logic.Logic.Query.GetUserQuery;

    public class GetUserController : BaseUserQueryController<IGetUserQuery, GetUserQueryRequest, GetUserQueryResponse>
    {
        public GetUserController(IGetUserQuery query) : base(query)
        {
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid id)
        {
            GetUserQueryResponse queryResponse = await this.query.ExecuteAsync(new GetUserQueryRequest { Id = id });
            GetUserControllerResponseModel response = queryResponse.ToControllerResponse();
            return this.Ok(response);
        }
    }
}