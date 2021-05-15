namespace users_logic.User.Provider
{
    using System;

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;
    }
}