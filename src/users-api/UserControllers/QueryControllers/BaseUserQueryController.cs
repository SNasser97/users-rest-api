namespace users_api.UserControllers.QueryControllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using users_api.Extensions;
    using users_api.UserControllers.QueryControllers.Models.Response;
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