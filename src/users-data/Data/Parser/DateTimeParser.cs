namespace users_data.Repositories.InMemoryUserRepository.Data.Parser
{
    using System;
    using System.Threading.Tasks;

    public class DateTimeParser : IDateTimeParser
    {
        public async Task<int> ParseDateTimeAsAgeAsync(DateTime current, DateTime dob)
        {
            return await Task.FromResult(current.Year - dob.Year);
        }
    }
}
