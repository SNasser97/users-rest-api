namespace users_data.Repositories
{
    using System;
    using System.Threading.Tasks;

    public interface IWriteRepository<TRecord, TRecordUpdate>
    {
        Task<Guid> CreateAsync(TRecord record);

        Task<Guid> UpdateAsync(TRecordUpdate record);

        Task DeleteAsync(Guid id);
    }
}
