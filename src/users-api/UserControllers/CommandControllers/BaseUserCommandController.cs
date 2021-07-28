namespace users_api.UserControllers.CommandControllers
{
    using Microsoft.AspNetCore.Mvc;
    using users_logic.Logic.Command;

    [ApiController]
    [Route("users")]
    public abstract class BaseUserCommandController<TCommand, TRequest, TResponse> : ControllerBase
        where TCommand : ICommand<TRequest, TResponse>
    {
        protected readonly TCommand command;

        protected BaseUserCommandController(TCommand command)
        {
            this.command = command;
        }
    }
}