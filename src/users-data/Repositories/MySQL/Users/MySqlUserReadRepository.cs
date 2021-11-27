namespace users_data.Repositories.MySQL
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;
    using MySql.Data.MySqlClient;
    using users_data.Entities;
    using users_data.Manager;
    using users_data.Repositories.MySQL.MySqlDataMapper;

    public class MySqlUserReadRepository : IReadRepository<User>
    {
        private readonly IDbConnectionManager connectionManager;
        private readonly ISqlDataMapper<User> sqlDataMapper;

        public MySqlUserReadRepository(IDbConnectionManager connectionManager, ISqlDataMapper<User> sqlDataMapper)
        {
            this.connectionManager = connectionManager;
            this.sqlDataMapper = sqlDataMapper;
        }

        public async Task<IEnumerable<User>> GetAsync()
        {
            MySqlConnection mySqlConnection = (MySqlConnection)await this.connectionManager.GetDbConnectionAsync();
            using (mySqlConnection)
            {
                await mySqlConnection.OpenAsync();
                MySqlCommand cmd = mySqlConnection.CreateCommand();
                // Debugging
                cmd.CommandText = "SELECT * FROM `users_db`.users LIMIT 1";
                var li = new List<User>();
                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync() && reader.HasRows)
                    {
                        li.Add(this.sqlDataMapper.MapDataToEntity(reader));
                    }
                }

                // Debugging
                Console.WriteLine("Collection => {0}", JsonSerializer.Serialize(li));
                Console.WriteLine("Size => {0}", li.Count);
                Console.WriteLine("Timestamp => {0}", DateTime.Now);
            }

            return await Task.FromResult(Enumerable.Empty<User>());
        }

        public Task<User> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}