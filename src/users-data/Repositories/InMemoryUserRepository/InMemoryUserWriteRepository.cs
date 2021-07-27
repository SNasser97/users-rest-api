namespace users_data.Repositories.InMemoryUserRepository
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using users_data.Entities;
    using users_data.Extensions;

    public class InMemoryUserWriteRepository : IWriteRepository<User>
    {
        private readonly IRecordData<User> recordData;

        public InMemoryUserWriteRepository(IRecordData<User> recordData)
        {
            this.recordData = recordData ?? throw new ArgumentNullException(nameof(recordData));
        }

        public async Task<Guid> CreateAsync(User record)
        {
            if (this.recordData.EntityStorage.TryAdd(record.Id, record))
            {
                return await Task.FromResult(record.Id);
            }

            return Guid.Empty;
        }

        public async Task DeleteAsync(Guid id)
            => await Task.FromResult(this.recordData.EntityStorage.Remove(id));

        public async Task<Guid> UpdateAsync(User record)
        {
            if (this.recordData.EntityStorage.TryGetValue(record.Id, out User found))
            {
                found.UpdateUserRecord(ref record);
                return await Task.FromResult(found.Id);
            };

            return Guid.Empty;
        }
    }
}
