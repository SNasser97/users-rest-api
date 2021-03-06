namespace users_logic.Facades
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using users_data.Entities;
    using users_logic.Parser;
    using users_logic.Provider;

    public class UserLogicFacade : IUserLogicFacade
    {
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IDateTimeParser dateTimeParser;

        public UserLogicFacade(
            IDateTimeProvider dateTimeProvider,
            IDateTimeParser dateTimeParser
        )
        {
            this.dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
            this.dateTimeParser = dateTimeParser ?? throw new ArgumentNullException(nameof(dateTimeParser));
        }

        public async Task<bool> DoesUserEmailAlreadyExistAsync(IEnumerable<User> records, string email)
            => await Task.FromResult(records.Any(u => u.Email == email));

        public async Task<bool> IsAgeValidAsync(int age)
            => await Task.FromResult(age >= 18 && age <= 110);

        public async Task<int> GetCalculatedUsersAgeAsync(DateTime dateOfBirth)
        {
            DateTime current = this.dateTimeProvider.Now;
            int age = await this.dateTimeParser.ParseDateOfBirthAsAgeAsync(current, dateOfBirth);
            return age;
        }
    }
}