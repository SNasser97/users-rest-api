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
        private readonly IRecordData<BaseUserRecordWithId> recordData;

        public InMemoryUserWriteRepository(IRecordData<BaseUserRecordWithId> recordData)
        {
            this.recordData = recordData ?? throw new ArgumentNullException(nameof(recordData));
        }

        public async Task<Guid> CreateAsync(BaseUserRecord record)
        {
            Guid recordId = Guid.NewGuid();

            UserRecord userRecord = record.ToUserRecord(recordId);

            if (this.recordData.Users.TryAdd(userRecord.Id, userRecord))
            {
                return await Task.FromResult(userRecord.Id);
            }
            Debug.WriteLine("Created: {0}", JsonSerializer.Serialize(userRecord));
            return await Task.FromResult(Guid.Empty);
        }

        public async Task DeleteAsync(Guid id)
        {
            await Task.FromResult(this.recordData.Users.Remove(id));
        }

        public async Task<Guid> UpdateAsync(BaseUserRecordWithId record)
        {
            if (this.recordData.Users.TryGetValue(record.Id, out BaseUserRecordWithId found))
            {
                found.UpdateUserRecord(ref record);
                return await Task.FromResult(found.Id);
            };

            return await Task.FromResult(Guid.Empty);
        }
    }
}
