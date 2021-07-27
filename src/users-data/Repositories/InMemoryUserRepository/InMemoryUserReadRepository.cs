namespace users_data.Repositories.InMemoryUserRepository
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using users_data.Entities;

    public class InMemoryUserReadRepository : IReadRepository<User>
    {
        private readonly IRecordData<User> recordData;

        public InMemoryUserReadRepository(IRecordData<User> recordData)
        {
            this.recordData = recordData ?? throw new ArgumentNullException(nameof(recordData));
        }

        public async Task<IEnumerable<User>> GetAsync()
            => await Task.FromResult(this.recordData.EntityStorage.Values);

        public async Task<User> GetAsync(Guid id)
        {
            this.recordData.EntityStorage.TryGetValue(id, out User userRecord);
            return await Task.FromResult(userRecord);
        }
    }
}