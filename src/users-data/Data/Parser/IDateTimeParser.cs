namespace users_data.Repositories.InMemoryUserRepository.Data.Parser
{
    using System;
    using System.Threading.Tasks;

    public interface IDateTimeParser
    {
        Task<int> ParseDateTimeAsAgeAsync(DateTime current, DateTime time);
    }
}
