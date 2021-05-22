namespace users_api.UserControllers.CommandControllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using users_api.Extensions;
    using users_api.UserControllers.CommandControllers.Models.Request;
    using users_api.UserControllers.CommandControllers.Models.Response;
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
            CreateUserControllerResponseModel response = await ControllerResponseModelExtensions.CaptureResponse<CreateUserControllerResponseModel, CreateUserCommandResponseModel>(async ()
                => (CreateUserCommandResponseModel)await this.userCommand.CreateUserAsync(request.ToUserCommandRequest()));

            if (!string.IsNullOrWhiteSpace(response?.Error))
            {
                return this.BadRequest(response.Error);
            }

            return this.Ok(response.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, UpdateUserControllerRequestModel request)
        {
            UpdateUserControllerResponseModel response = await ControllerResponseModelExtensions.CaptureResponse<UpdateUserControllerResponseModel, UpdateUserCommandResponseModel>(async ()
                => (UpdateUserCommandResponseModel)await this.userCommand.UpdateUserAsync(request.ToUserCommandRequest(id)));

            if (!string.IsNullOrWhiteSpace(response?.Error))
            {
                return this.BadRequest(response.Error);
            }

            return this.Ok(response.Id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await this.userCommand.DeleteUserAsync(new DeleteUserCommandRequestModel { Id = id });
            return this.Ok();
        }
    }
}