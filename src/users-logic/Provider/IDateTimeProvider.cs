namespace users_logic.Provider
{
    using System;

    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}