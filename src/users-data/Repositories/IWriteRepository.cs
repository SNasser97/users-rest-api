namespace users_data.Repositories
{
    using System;
    using System.Threading.Tasks;

    public interface IWriteRepository<TRecord, T2Record>
    {
        Task<Guid> CreateAsync(TRecord record);

        Task<Guid> UpdateAsync(T2Record record);

        Task DeleteAsync(Guid id);
    }
}
