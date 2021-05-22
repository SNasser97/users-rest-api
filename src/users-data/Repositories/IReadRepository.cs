namespace users_data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IReadRepository<TRecord>
    {
        Task<IEnumerable<TRecord>> GetAsync();

        Task<TRecord> GetAsync(Guid id);
    }
}
