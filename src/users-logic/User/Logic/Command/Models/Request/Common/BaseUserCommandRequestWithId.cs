namespace users_logic.User.Logic.Command.Models.Request.Common
{
    using System;

    public abstract class BaseUserCommandRequestWithId : BaseUserCommandRequest
    {
        public Guid Id { get; set; }
    }
}