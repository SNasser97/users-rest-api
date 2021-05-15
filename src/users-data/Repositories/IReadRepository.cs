namespace users_data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IReadRepository<TRecord>
    {
        // Return a collection of TRecord
        Task<IEnumerable<TRecord>> GetAsync();

        // Return single TRecord
        Task<TRecord> GetAsync(Guid id);
    }
}
