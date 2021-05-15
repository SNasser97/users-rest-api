namespace users_data.Repositories.InMemoryUserRepository.Data.Provider
{
    using System;

    public class DateTimeProvider : IDateTimeProvider
    {
        // For mocking
        public DateTime Now { get => DateTime.Now; }
    }
}
