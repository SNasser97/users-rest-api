namespace users_data.Repositories
{
    using System;
    using System.Threading.Tasks;

    public interface IWriteRepository<TRecord>
    {
        // Create TRecord
        Task<Guid> CreateAsync(TRecord record);

        // Updata TRecord
        Task<Guid> UpdateAsync(TRecord record);

        // Delete TRecord
        Task DeleteAsync(Guid id);
    }
}
