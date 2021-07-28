namespace users_api.UserControllers.CommandControllers.DeleteUserController
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using users_logic.Logic.Command.DeleteUserCommand;

    public class DeleteUserController : BaseUserCommandController<IDeleteUserCommand, DeleteUserCommandRequest, DeleteUserCommandResponse>
    {
        public DeleteUserController(IDeleteUserCommand command) : base(command)
        {
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await this.command.ExecuteAsync(new DeleteUserCommandRequest { Id = id });
            return this.NoContent();
        }
    }
}