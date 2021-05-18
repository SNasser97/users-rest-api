namespace users_api.UserControllers.QueryControllers
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using users_logic.User.Logic.Query;
    using users_logic.User.Logic.Query.Models.Request;
    using users_logic.User.Logic.Query.Models.Response;

    [ApiController]
    [Route("[controller]")]
    public class UserQueryController : ControllerBase
    {
        private readonly IUserQuery<GetUserQueryRequestModel, GetUserQueryResponseModel, GetUsersQueryResponseModel> userQuery;
        public UserQueryController(
            IUserQuery<GetUserQueryRequestModel, GetUserQueryResponseModel, GetUsersQueryResponseModel> userQuery)
        {
            this.userQuery = userQuery ?? throw new System.ArgumentNullException(nameof(userQuery));
        }

        [HttpGet("users")]
        public async Task<GetUsersQueryResponseModel> GetAsync()
        {
            var a = await this.userQuery.GetResponsesAsync();
            Debug.WriteLine(JsonSerializer.Serialize(a.Users));
            return a;
        }
    }
}