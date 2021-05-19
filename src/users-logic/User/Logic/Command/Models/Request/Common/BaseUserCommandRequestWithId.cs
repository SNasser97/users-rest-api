namespace users_logic.User.Logic.Command.Models.Request.Common
{
    using System;

    public abstract class BaseUserCommandRequestWithIdModel : BaseUserCommandRequestModel
    {
        public Guid Id { get; set; }
    }
}