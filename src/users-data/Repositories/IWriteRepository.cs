namespace users_data.Repositories
{
    using System;
    using System.Threading.Tasks;

    public interface IWriteRepository<TRecord, T2Record>
    {
        // Create TRecord
        Task<Guid> CreateAsync(TRecord record);

        // Updata TRecord
        Task<Guid> UpdateAsync(T2Record record);

        // Delete TRecord
        Task DeleteAsync(Guid id);
    }
}
