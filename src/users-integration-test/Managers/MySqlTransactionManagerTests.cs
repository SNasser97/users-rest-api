namespace users_integration_test.Managers
{
    using System;
    using System.Threading.Tasks;
    using MySql.Data.MySqlClient;
    using users_data.Repositories.MySQL.MySqlManagers;
    using users_test.Helper;
    using Xunit;

    public class MySqlTransactionManagerTests
    {
        private Lazy<string> connectionStringTestValue = new Lazy<string>(Environment.GetEnvironmentVariable("MYSQL_CONNECTION") ?? $"Server=localhost;Uid=admin;Pwd=secret;Database=users_db;");

        [Fact]
        public async Task MySqlTransactionManagerExecutesTransactionExpectsReturnedValue()
        {
            var connectionManager = new MySqlConnectionManager(connectionStringTestValue);
            var transactionManager = new MySqlTransactionManager(connectionManager);

            Guid actualResult = await transactionManager.ExecuteTransactionAsync(async (transaction) =>
            {
                MySqlTransaction sqlTransaction = transaction as MySqlTransaction;
                Assert.NotNull(sqlTransaction);
                Assert.False(string.IsNullOrWhiteSpace(sqlTransaction.Connection.ConnectionString));

                (string actualServer, string actualUid, string actualDb) = this.GetConnectionValues(sqlTransaction.Connection.ConnectionString.Split(';'));
                Assert.Contains("localhost", actualServer);
                Assert.Contains("admin", actualUid);
                Assert.Contains("users_db", actualDb);
                return await Task.FromResult(Guid.NewGuid());
            });

            Assert.True(actualResult != Guid.Empty);
        }

        [Fact]
        public async Task MySqlTransactionManagerExecutesTransactionThrowsMySqlException()
        {
            var connectionManager = new MySqlConnectionManager(new Lazy<string>(""));
            var transactionManager = new MySqlTransactionManager(connectionManager);

            await Exceptions<MySqlException>.HandleAsync(async () =>
            {
                await transactionManager.ExecuteTransactionAsync(async (transaction) => await Task.FromResult(Guid.NewGuid()));
            }, (ex) => Assert.Contains("Access denied for user", ex.Message));
        }

        private Tuple<string, string, string> GetConnectionValues(string[] values)
            => new Tuple<string, string, string>(values[0], values[1], values[2]);
    }
}