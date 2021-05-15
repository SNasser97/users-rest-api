namespace users_data.Repositories.InMemoryUserRepository.Data.Provider
{
    using System;

    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}
