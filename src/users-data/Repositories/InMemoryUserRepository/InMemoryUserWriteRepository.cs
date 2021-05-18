namespace users_data.Repositories.InMemoryUserRepository
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text.Json;
    using System.Threading.Tasks;
    using users_data.Entities;
    using users_data.Extensions;

    public class InMemoryUserWriteRepository : IWriteRepository<BaseUserRecord, BaseUserRecordWithId>
    {
        private readonly IDictionary<Guid, UserRecord> users;

        public InMemoryUserWriteRepository() : this(
            new Dictionary<Guid, UserRecord>())
        {
        }

        public InMemoryUserWriteRepository(
            IDictionary<Guid, UserRecord> users)
        {
            this.users = users ?? throw new ArgumentNullException(nameof(users));
        }

        public async Task<Guid> CreateAsync(BaseUserRecord record)
        {
            Guid recordId = Guid.NewGuid();

            UserRecord userRecord = record.ToUserRecord(recordId);

            if (this.users.TryAdd(userRecord.Id, userRecord))
            {
                return await Task.FromResult(userRecord.Id);
            }
            Debug.WriteLine("Created: {0}", JsonSerializer.Serialize(userRecord));
            return await Task.FromResult(Guid.Empty);
        }

        public async Task DeleteAsync(Guid id)
        {
            await Task.FromResult(this.users.Remove(id));
        }

        public async Task<Guid> UpdateAsync(BaseUserRecordWithId record)
        {
            if (this.users.TryGetValue(record.Id, out UserRecord found))
            {
                found.UpdateUserRecord(ref record);

                return await Task.FromResult(found.Id);
            };

            return await Task.FromResult(Guid.Empty);
        }
    }
}
