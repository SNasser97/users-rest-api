namespace users_data.Repositories.InMemoryUserRepository.Data.Validation
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IVerifyRecord<TRecord>
    {
        Task<bool> IsUserRecordValidAgeAsync(int age);

        Task<bool> DoesEmailExistAsync(IEnumerable<TRecord> records, string email);
    }
}
