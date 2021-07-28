namespace users_api.UserControllers.CommandControllers.UpdateUserController
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using users_api.Extensions;
    using users_api.UserControllers.CommandControllers.Models.Request;
    using users_logic.Logic.Command.UpdateUserCommand;

    public class UpdateUserController : BaseUserCommandController<IUpdateUserCommand, UpdateUserCommandRequest, UpdateUserCommandResponse>
    {
        public UpdateUserController(IUpdateUserCommand command) : base(command)
        {
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, UpdateUserControllerRequestModel request)
        {
            UpdateUserCommandResponse response = await this.command.ExecuteAsync(request.ToCommandRequest(id));
            return this.Ok(response.Id);
        }
    }
}