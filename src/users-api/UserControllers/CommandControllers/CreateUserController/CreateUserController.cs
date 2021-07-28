namespace users_api.UserControllers.CommandControllers.CreateUserController
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using users_api.Extensions;
    using users_api.UserControllers.CommandControllers.Models.Request;
    using users_logic.Logic.Command.CreateUserCommand;

    public class CreateUserController : BaseUserCommandController<ICreateUserCommand, CreateUserCommandRequest, CreateUserCommandResponse>
    {
        public CreateUserController(ICreateUserCommand command) : base(command)
        {
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateUserControllerRequestModel request)
        {
            CreateUserCommandResponse response = await this.command.ExecuteAsync(request.ToCommandRequest());
            return this.Ok(response.Id);
        }
    }
}