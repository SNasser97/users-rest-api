namespace users_data.Repositories.InMemoryUserRepository
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using users_data.Entities;

    public class InMemoryUserWriteRepository : IWriteRepository
    {
        private readonly IDictionary<Guid, UserRecord> users;

        // mocking usage
        public InMemoryUserWriteRepository() : this(
            new Dictionary<Guid, UserRecord>())
        {
        }

        public InMemoryUserWriteRepository(
            IDictionary<Guid, UserRecord> users)
        {
            this.users = users ?? throw new ArgumentNullException(nameof(users));
        }

        public async Task<Guid> CreateAsync(CreateUserRecord record)
        {
            Guid recordId = Guid.NewGuid();

            UserRecord userRecord = new UserRecord
            {
                Id = recordId,
                FirstName = record.FirstName,
                LastName = record.LastName,
                Email = record.Email,
                DateOfBirth = record.DateOfBirth,
                Age = record.Age
            };

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

        public async Task<Guid> UpdateAsync(UpdateUserRecord record)
        {
            if (this.users.TryGetValue(record.Id, out UserRecord found))
            {
                found.FirstName = record.FirstName;
                found.LastName = record.LastName;
                found.Email = record.Email;
                found.DateOfBirth = record.DateOfBirth;
                found.Age = record.Age;

                return await Task.FromResult(found.Id);
            };

            return await Task.FromResult(Guid.Empty);
        }
    }
}
