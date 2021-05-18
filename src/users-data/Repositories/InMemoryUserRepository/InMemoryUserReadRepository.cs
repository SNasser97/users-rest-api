namespace users_data.Repositories.InMemoryUserRepository
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using users_data.Entities;

    public class InMemoryUserReadRepository : IReadRepository<BaseUserRecordWithId>
    {
        private readonly IDictionary<Guid, BaseUserRecordWithId> users;

        public InMemoryUserReadRepository() : this(new Dictionary<Guid, BaseUserRecordWithId>())
        {
        }

        public InMemoryUserReadRepository(IDictionary<Guid, BaseUserRecordWithId> users)
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
    }
}