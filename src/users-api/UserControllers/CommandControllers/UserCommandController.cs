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
            CreateUserControllerResponseModel createResponse = await ControllerResponseModelExtensions.CaptureResponseAsync<CreateUserControllerResponseModel, CreateUserCommandResponseModel>(async ()
                => (CreateUserCommandResponseModel)await this.userCommand.CreateUserAsync(request.ToUserCommandRequest()));

            if (!string.IsNullOrWhiteSpace(createResponse?.Error))
            {
                return this.BadRequest(createResponse.Error);
            }

            return this.Ok(createResponse.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, UpdateUserControllerRequestModel request)
        {
            UpdateUserControllerResponseModel updateResponse = await ControllerResponseModelExtensions.CaptureResponseAsync<UpdateUserControllerResponseModel, UpdateUserCommandResponseModel>(async ()
                => (UpdateUserCommandResponseModel)await this.userCommand.UpdateUserAsync(request.ToUserCommandRequest(id)));

            if (!string.IsNullOrWhiteSpace(updateResponse?.Error))
            {
                return this.BadRequest(updateResponse.Error);
            }

            return this.Ok(updateResponse.Id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            DeleteUserControllerResponseModel deleteResponse = await ControllerResponseModelExtensions.CaptureDeleteResponseAsync(async ()
                => await this.userCommand.DeleteUserAsync(new DeleteUserCommandRequestModel { Id = id }));

            if (!string.IsNullOrWhiteSpace(deleteResponse?.Error))
            {
                return this.BadRequest(deleteResponse.Error);
            }

            return this.Ok();
        }
    }
}