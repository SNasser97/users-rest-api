namespace users_logic.Parser
{
    using System;
    using System.Threading.Tasks;

    public class DateTimeParser : IDateTimeParser
    {
        public async Task<int> ParseDateOfBirthAsAgeAsync(DateTime currentTime, DateTime dateOfBirth)
        {
            int age = currentTime.Year - dateOfBirth.Year;

            // Leap year
            if (dateOfBirth.Date > currentTime.AddYears((-age)))
            {
                age--;
            }

            return await Task.FromResult(age);
        }
    }
}