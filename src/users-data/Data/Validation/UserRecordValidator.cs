namespace users_data.Repositories.InMemoryUserRepository.Data.Validation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using users_data.Models;

    public class UserRecordValidator : IVerifyRecord<UserRecord>
    {
        public async Task<bool> DoesEmailExistAsync(IEnumerable<UserRecord> records, string email)
            => await Task.FromResult(records.Any(r => r.Email == email));

        public async Task<bool> IsUserRecordValidAgeAsync(int age)
            => await Task.FromResult(age >= 18 && age <= 110);
    }
}
