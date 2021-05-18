namespace users_api.UserControllers.CommandControllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using users_api.UserControllers.CommandControllers.Models.Request;
    using users_logic.Exceptions.Validation;
    using users_logic.User.Logic.Command;
    using users_logic.User.Logic.Command.Models.Response;

    [ApiController]
    [Route("[controller]")]
    public class UserCommandController : ControllerBase
    {
        private readonly IUserCommand<BaseUserCommandResponse> userCommand;

        public UserCommandController(IUserCommand<BaseUserCommandResponse> userCommand)
        {
            this.userCommand = userCommand;
        }

        [HttpPost("users")]
        public async Task<IActionResult> Create([FromBody] CreateUserControllerRequestModel request)
        {
            try
            {
                CreateUserCommandResponse a = (CreateUserCommandResponse)await this.userCommand.CreateUserAsync(request.ToCommandRequest());
                return this.Ok(a.Id);

            }
            catch (EmailExistsException ex)
            {
                return this.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
    }


}