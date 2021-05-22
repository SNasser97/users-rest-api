namespace users_api.UserControllers.QueryControllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using users_api.Extensions;
    using users_api.UserControllers.QueryControllers.Models.Response;
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
        public async Task<GetUserControllerResponsesModel> GetAsync()
        {
            GetUsersQueryResponseModel userQueryResponses = await this.userQuery.GetResponsesAsync();
            IEnumerable<GetUserControllerResponseModel> responseModels = await userQueryResponses.Users.MapListToControllerResponsesAsync();
            return new GetUserControllerResponsesModel { Users = responseModels };
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid id)
        {
            GetUserControllerResponseModel getUserResponse = await ControllerResponseModelExtensions.CaptureGetResponseAsync(async ()
                => await this.userQuery.GetResponseAsync(new GetUserQueryRequestModel { Id = id }));

            if (!string.IsNullOrWhiteSpace(getUserResponse?.Error))
            {
                return this.BadRequest(getUserResponse.Error);
            }

            return this.Ok(getUserResponse);
        }
    }
}