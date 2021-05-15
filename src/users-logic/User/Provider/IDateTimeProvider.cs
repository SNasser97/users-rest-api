namespace users_logic.User.Provider
{
    using System;

    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}