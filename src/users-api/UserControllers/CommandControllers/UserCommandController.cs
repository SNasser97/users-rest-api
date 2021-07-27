namespace users_api.UserControllers.CommandControllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using users_api.Extensions;
    using users_api.UserControllers.CommandControllers.Models.Request;
    using users_logic.Logic.Command.CreateUserCommand;
    using users_logic.Logic.Command.DeleteUserCommand;
    using users_logic.Logic.Command.UpdateUserCommand;

    [ApiController]
    [Route("users")]
    public class UserCommandController : ControllerBase
    {
        private readonly ICreateUserCommand createUserCommand;
        private readonly IUpdateUserCommand updateUserCommand;
        private readonly IDeleteUserCommand deleteUserCommand;

        public UserCommandController(
            ICreateUserCommand createUserCommand,
            IUpdateUserCommand updateUserCommand,
            IDeleteUserCommand deleteUserCommand)
        {
            this.createUserCommand = createUserCommand ?? throw new ArgumentNullException(nameof(createUserCommand));
            this.updateUserCommand = updateUserCommand ?? throw new ArgumentNullException(nameof(updateUserCommand));
            this.deleteUserCommand = deleteUserCommand ?? throw new ArgumentNullException(nameof(deleteUserCommand));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateUserControllerRequestModel request)
        {
            CreateUserCommandResponse response = await this.createUserCommand.ExecuteAsync(request.ToCommandRequest());
            return this.Ok(response.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, UpdateUserControllerRequestModel request)
        {
            UpdateUserCommandResponse response = await this.updateUserCommand.ExecuteAsync(request.ToCommandRequest(id));
            return this.Ok(response.Id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await this.deleteUserCommand.ExecuteAsync(new DeleteUserCommandRequest { Id = id });
            return this.NoContent();
        }
    }
}