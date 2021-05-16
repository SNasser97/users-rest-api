namespace users_data.Repositories
{
    using System;
    using System.Threading.Tasks;
    using users_data.Entities;

    public interface IWriteRepository
    {
        // Create TRecord
        Task<Guid> CreateAsync(CreateUserRecord record);

        // Updata TRecord
        Task<Guid> UpdateAsync(UpdateUserRecord record);

        // Delete TRecord
        Task DeleteAsync(Guid id);
    }
}
