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

    public class MySqlUserReadRepository : IReadRepository<User>
    {
        private readonly IDbConnectionManager connectionManager;

        public MySqlUserReadRepository(IDbConnectionManager connectionManager)
        {
            this.connectionManager = connectionManager;
        }

        public async Task<IEnumerable<User>> GetAsync()
        {
            MySqlConnection mySqlConnection = (MySqlConnection)await this.connectionManager.GetDbConnectionAsync();
            using (mySqlConnection)
            {
                await mySqlConnection.OpenAsync();
                Console.WriteLine($"conn state use (0 is closed ~ 1 is open) => {mySqlConnection.State}");

                // Console.WriteLine($"conn state open (0 is closed ~ 1 is open) => {connection.State}");

                MySqlCommand cmd = mySqlConnection.CreateCommand();
                cmd.CommandText = "SELECT * FROM `users_db`.users";
                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync() && reader.HasRows)
                    {
                        Console.WriteLine("yes data found");
                        // Console.WriteLine($"{reader.GetString(c)}");
                        Console.WriteLine($"{JsonSerializer.Serialize(this.displayer(reader))}");
                    }

                    Console.WriteLine("no data found");
                }
            }

            Console.WriteLine($"conn state (0 is closed ~ 1 is open) => {mySqlConnection.State}");
            return await Task.FromResult(Enumerable.Empty<User>());
        }

        public async Task<User> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        private object[] displayer(DbDataReader reader)
        {
            return new object[] { reader["Id"].ToString(), reader["Firstname"].ToString(), reader["Lastname"].ToString(), reader["Email"].ToString(), reader["DateOfBirth"].ToString(), reader["Age"].ToString() };
        }
    }
}