namespace users_api.UserControllers.QueryControllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using users_logic.User.Logic.Query;
    using users_logic.User.Logic.Query.Models.Request;
    using users_logic.User.Logic.Query.Models.Response;

    [ApiController]
    [Route("users")]
    public class UserQueryController : ControllerBase
    {
        private readonly IUserQuery<GetUserQueryRequestModel, GetUserQueryResponseModel, GetUsersQueryResponseModel> userQuery;
        public UserQueryController(
            IUserQuery<GetUserQueryRequestModel, GetUserQueryResponseModel, GetUsersQueryResponseModel> userQuery)
        {
            this.userQuery = userQuery ?? throw new System.ArgumentNullException(nameof(userQuery));
        }

        [HttpGet]
        public async Task<GetUsersQueryResponseModel> GetAsync()
        {
            GetUsersQueryResponseModel userQueryResponses = await this.userQuery.GetResponsesAsync();
            return userQueryResponses;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid id)
        {
            GetUserQueryResponseModel userQueryResponse = await this.userQuery.GetResponseAsync(new GetUserQueryRequestModel { Id = id });
            return this.Ok(userQueryResponse);
        }
    }
}