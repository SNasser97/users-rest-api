using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using users_data.Entities;
using users_data.Extensions;

namespace users_data.Repositories.InMemoryUserRepository
{
    public class InMemoryUsersRepository : IReadRepository<BaseUserRecordWithId>, IWriteRepository<BaseUserRecord, BaseUserRecordWithId>
    {
        private readonly IDictionary<Guid, BaseUserRecordWithId> users;

        public InMemoryUsersRepository() : this(new Dictionary<Guid, BaseUserRecordWithId>())
        {
        }

        public InMemoryUsersRepository(IDictionary<Guid, BaseUserRecordWithId> users)
        {
            this.users = users ?? throw new ArgumentNullException(nameof(users));
        }

        public async Task<IEnumerable<BaseUserRecordWithId>> GetAsync()
            => await Task.FromResult(this.users.Values);

        public async Task<BaseUserRecordWithId> GetAsync(Guid id)
        {
            this.users.TryGetValue(id, out BaseUserRecordWithId userRecord);

            return await Task.FromResult(userRecord);
        }

        public async Task<Guid> CreateAsync(BaseUserRecord record)
        {
            Guid recordId = Guid.NewGuid();

            BaseUserRecordWithId userRecord = record.ToUserRecord(recordId);

            if (this.users.TryAdd(userRecord.Id, userRecord))
            {
                return await Task.FromResult(userRecord.Id);
            }

            return await Task.FromResult(Guid.Empty);
        }

        public async Task DeleteAsync(Guid id)
        {
            await Task.FromResult(this.users.Remove(id));
        }

        public async Task<Guid> UpdateAsync(BaseUserRecordWithId record)
        {
            if (this.users.TryGetValue(record.Id, out BaseUserRecordWithId found))
            {
                found.UpdateUserRecord(ref record);

                return await Task.FromResult(found.Id);
            };

            return await Task.FromResult(Guid.Empty);
        }
    }
}