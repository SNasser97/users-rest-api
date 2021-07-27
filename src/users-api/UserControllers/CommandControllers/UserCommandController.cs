namespace users_api.UserControllers.CommandControllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using users_api.Extensions;
    using users_api.UserControllers.CommandControllers.Models.Request;
    using users_logic.User.Logic.Command;
    using users_logic.User.Logic.Command.Models.Request;
    using users_logic.User.Logic.Command.Models.Response;

    [ApiController]
    [Route("users")]
    public class UserCommandController : ControllerBase
    {
        private readonly IUserCommand<BaseUserCommandResponseModel> userCommand;

        public UserCommandController(IUserCommand<BaseUserCommandResponseModel> userCommand)
        {
            this.userCommand = userCommand ?? throw new ArgumentNullException(nameof(userCommand));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateUserControllerRequestModel request)
        {
            CreateUserCommandResponseModel response = (CreateUserCommandResponseModel)await this.userCommand.CreateUserAsync(request.ToUserCommandRequest());
            return this.Ok(response.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, UpdateUserControllerRequestModel request)
        {
            UpdateUserCommandResponseModel response = (UpdateUserCommandResponseModel)await this.userCommand.UpdateUserAsync(request.ToUserCommandRequest(id));
            return this.Ok(response.Id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await this.userCommand.DeleteUserAsync(new DeleteUserCommandRequestModel { Id = id });
            return this.NoContent();
        }
    }
}