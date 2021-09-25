namespace users_data.Repositories.MySQL
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using users_data.Entities;

    public class MySqlUserWriteRepository : IWriteRepository<User>
    {

        public MySqlUserWriteRepository()
        {
            // db transaction
        }

        // transaction here    
        public Task<Guid> CreateAsync(User record)
        {
            throw new NotImplementedException();
        }

        // transaction here 
        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        // transaction here 
        public Task<Guid> UpdateAsync(User record)
        {
            throw new NotImplementedException();
        }
    }
}