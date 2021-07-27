namespace users_data.Repositories
{
    using System;
    using System.Threading.Tasks;

    public interface IWriteRepository<TRecord>
    {
        Task<Guid> CreateAsync(TRecord record);

        Task<Guid> UpdateAsync(TRecord record);

        Task DeleteAsync(Guid id);
    }
}
