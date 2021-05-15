namespace users_data.Repositories.InMemoryUserRepository
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using users_data.Entities;

    public class InMemoryUserReadRepository : IReadRepository<UserRecord>
    {
        private readonly IDictionary<Guid, UserRecord> users;

        public InMemoryUserReadRepository() : this(new Dictionary<Guid, UserRecord>())
        {
        }

        public InMemoryUserReadRepository(IDictionary<Guid, UserRecord> users)
        {
            this.users = users ?? throw new ArgumentNullException(nameof(users));
        }

        public async Task<IEnumerable<UserRecord>> GetAsync()
            => await Task.FromResult(this.users.Values);

        public async Task<UserRecord> GetAsync(Guid id)
        {
            this.users.TryGetValue(id, out UserRecord userRecord);

            return await Task.FromResult(userRecord);
        }
    }
}