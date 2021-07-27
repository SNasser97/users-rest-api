namespace users_logic.Parser
{
    using System;
    using System.Threading.Tasks;

    public class DateTimeParser : IDateTimeParser
    {
        public async Task<int> ParseDateOfBirthAsAgeAsync(DateTime currentTime, DateTime dateOfBirth)
            => await Task.FromResult(currentTime.Year - dateOfBirth.Year);
    }
}