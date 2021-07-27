namespace users_api.UserControllers.QueryControllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using users_api.Extensions;
    using users_api.UserControllers.QueryControllers.Models.Response;
    using users_logic.Logic.Query;
    using users_logic.Logic.Query.GetUserQuery;
    using users_logic.Logic.Query.GetUsersQuery;

    [ApiController]
    [Route("users")]
    public class UserQueryController : ControllerBase
    {
        private readonly IGetUserQuery getUserQuery;
        private readonly IGetUsersQuery getUsersQuery;

        public UserQueryController(IGetUserQuery getUserQuery, IGetUsersQuery getUsersQuery)
        {
            this.getUserQuery = getUserQuery ?? throw new ArgumentNullException(nameof(getUserQuery));
            this.getUsersQuery = getUsersQuery ?? throw new ArgumentNullException(nameof(getUsersQuery));
        }

        [HttpGet]
        public async Task<GetUserControllerResponsesModel> GetAsync()
        {
            IEnumerable<GetUserQueryResponse> userQueryResponses = await this.getUsersQuery.ExecuteAsync(null as GetUserQueryRequest);
            // TODO: Rework response models
            IEnumerable<GetUserControllerResponseModel> responseModels = await userQueryResponses.MapListToControllerResponsesAsync();
            return new GetUserControllerResponsesModel { Users = responseModels };
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid id)
        {
            GetUserQueryResponse getUserResponse = await this.getUserQuery.ExecuteAsync(new GetUserQueryRequest { Id = id });
            return this.Ok(getUserResponse);
        }
    }
}