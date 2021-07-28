namespace users_api.UserControllers.QueryControllers
{
    using Microsoft.AspNetCore.Mvc;
    using users_logic.Logic.Query;

    [ApiController]
    [Route("users")]
    public abstract class BaseUserQueryController<TQuery, TRequest, TResponse> : ControllerBase
        where TQuery : IQuery<TRequest, TResponse>
    {
        protected readonly TQuery query;

        protected BaseUserQueryController(TQuery query)
        {
            this.query = query;
        }


    }
}