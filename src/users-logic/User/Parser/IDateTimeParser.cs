namespace users_logic.User.Parser
{
    using System;
    using System.Threading.Tasks;

    public interface IDateTimeParser
    {
        /// <param name="currentTime">Current date to use as the base for calculating age</param>
        /// <param name="dateOfBirth">The DOB property from requested record</param>
        Task<int> ParseDateOfBirthAsAgeAsync(DateTime currentTime, DateTime dateOfBirth);
    }
}