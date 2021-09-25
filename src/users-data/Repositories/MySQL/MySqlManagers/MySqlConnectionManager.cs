namespace users_data.Repositories.MySQL.MySqlManagers
{
    using System;
    using System.Data.Common;
    using System.Threading;
    using System.Threading.Tasks;
    using MySql.Data.MySqlClient;
    using users_data.Manager;

    public class MySqlConnectionManager : IDbConnectionManager
    {
        private readonly Lazy<string> mysqlConnectionString;

        private AsyncLocal<MySqlConnection> connection => new AsyncLocal<MySqlConnection>
        {
            Value = new MySqlConnection(this.mysqlConnectionString.Value)
        };

        public MySqlConnectionManager(Lazy<string> mysqlConnectionString)
        {
            this.mysqlConnectionString = mysqlConnectionString;
        }

        public MySqlConnectionManager() : this(new Lazy<string>(Environment.GetEnvironmentVariable("MYSQL_CONNECTION")))
        {
        }

        public async Task CloseDbConnectionAsync(DbConnection connection)
        {
            await connection.CloseAsync();
        }

        public async Task<DbConnection> GetDbConnectionAsync()
            => await Task.FromResult(this.connection.Value);
    }
}