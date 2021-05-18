namespace users_data.Repositories.InMemoryUserRepository
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using users_data.Entities;

    public class InMemoryUserReadRepository : IReadRepository<BaseUserRecordWithId>
    {
        private readonly IRecordData<BaseUserRecordWithId> recordData;

        // public InMemoryUserReadRepository() : this(new InMemoryUsersRecordData())
        // {
        // }

        public InMemoryUserReadRepository(IRecordData<BaseUserRecordWithId> recordData)
        {
            this.recordData = recordData ?? throw new ArgumentNullException(nameof(recordData));
        }

        public async Task<IEnumerable<BaseUserRecordWithId>> GetAsync()
            => await Task.FromResult(this.recordData.Users.Values);

        public async Task<BaseUserRecordWithId> GetAsync(Guid id)
        {
            this.recordData.Users.TryGetValue(id, out BaseUserRecordWithId userRecord);

            return await Task.FromResult(userRecord);
        }
    }
}