namespace users_data.Repositories.InMemoryUserRepository
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using users_data.Models;

    public class InMemoryUserWriteRepository : IWriteRepository<UserRecord>
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

        public async Task<Guid> CreateAsync(UserRecord record)
        {
            if (this.users.TryAdd(record.Id, record))
            {
                return await Task.FromResult(record.Id);
            }

            return await Task.FromResult(Guid.Empty);
        }

        public async Task DeleteAsync(Guid id)
        {
            await Task.FromResult(this.users.Remove(id));
        }

        public async Task<Guid> UpdateAsync(UserRecord record)
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
