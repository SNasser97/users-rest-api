namespace users_data.Facades
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using users_data.Models;
    using users_data.Repositories.InMemoryUserRepository.Data.Parser;
    using users_data.Repositories.InMemoryUserRepository.Data.Provider;
    using users_data.Repositories.InMemoryUserRepository.Data.Validation;

    public class UserFacade : IUserFacade
    {
        private readonly IVerifyRecord<UserRecord> verifyRecord;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IDateTimeParser dateTimeParser;

        public UserFacade() : this(
            new UserRecordValidator(),
            new DateTimeProvider(),
            new DateTimeParser())
        {
        }

        public UserFacade(
            IVerifyRecord<UserRecord> verifyRecord,
            IDateTimeProvider dateTimeProvider,
            IDateTimeParser dateTimeParser
        )
        {
            this.verifyRecord = verifyRecord ?? throw new ArgumentNullException(nameof(verifyRecord));
            this.dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
            this.dateTimeParser = dateTimeParser ?? throw new ArgumentNullException(nameof(dateTimeParser));
        }

        public async Task<bool> CanUserBeInsertedAsync(IEnumerable<UserRecord> records, IUserRecord record)
        {
            int age = await this.dateTimeParser.ParseDateTimeAsAgeAsync(this.dateTimeProvider.Now, record.DateOfBirth);
            bool isValidAge = await this.verifyRecord.IsUserRecordValidAgeAsync(age);
            bool doesEmailExistAlready = await this.verifyRecord.DoesEmailExistAsync(records, record.Email);

            if (!isValidAge || doesEmailExistAlready)
            {
                return await Task.FromResult(false);
            }

            return true;
        }

        public Task<bool> DoesUserRecordEmailExistAsync(IEnumerable<UserRecord> records, IUserRecord record)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetUserAgeAsync(DateTime dob)
            => await this.dateTimeParser.ParseDateTimeAsAgeAsync(this.dateTimeProvider.Now, dob);

        public Task<bool> IsUserRecordAgeValidAsync(int age)
        {
            throw new NotImplementedException();
        }
    }
}