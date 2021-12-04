namespace users_integration_test.Managers.Managers
{
    using System;
    using System.Threading.Tasks;
    using MySql.Data.MySqlClient;
    using users_data.Repositories.MySQL.MySqlManagers;
    using Xunit;
    using users_test.Helper;

    public class MySqlConnectionManagerTests
    {
        private string connectionErrorMessage = "Access denied for user";
        private string noMySqlConnectionMessage = "Unable to connect to any of the specified MySQL hosts.";
        private Lazy<string> connectionStringTestValue = new Lazy<string>(Environment.GetEnvironmentVariable("MYSQL_CONNECTION") ?? $"Server=localhost;Uid=admin;Pwd=secret;Database=users_db;");

        [Fact]
        public async Task MySqlConnectionManagerGetDbConnectionAsyncReturnsInvalidMySqlConnectionEntity()
        {
            // Given

            var connectionString = new Lazy<string>("");
            var connectionManager = new MySqlConnectionManager(connectionString);

            // When
            MySqlConnection actualConnection = (MySqlConnection)await connectionManager.GetDbConnectionAsync();

            // Then
            Assert.True(string.IsNullOrWhiteSpace(actualConnection.Database));
            Assert.Equal("Closed", actualConnection.State.ToString());
            Assert.NotNull(actualConnection);

            await Exceptions<MySqlException>.HandleAsync(async () =>
            {
                using (actualConnection)
                {
                    await actualConnection.OpenAsync();
                }
            }, (ex) => Assert.True(true == ex.Message.Contains(connectionErrorMessage)));
        }

        [Fact]
        public async Task MySqlConnectionManagerGetDbConnectionAsyncReturnsValidMySqlConnectionEntity()
        {
            // Given
            var connectionManager = new MySqlConnectionManager(connectionStringTestValue);

            // When
            MySqlConnection actualConnection = (MySqlConnection)await connectionManager.GetDbConnectionAsync();

            // Then
            Assert.True(!string.IsNullOrWhiteSpace(actualConnection.Database));
            Assert.Equal("Closed", actualConnection.State.ToString());
            Assert.NotNull(actualConnection);

            await Exceptions<MySqlException>.HandleAsync(async () =>
            {
                using (actualConnection)
                {
                    await actualConnection.OpenAsync();
                    Assert.Equal("Open", actualConnection.State.ToString());
                }

                Assert.Equal("Closed", actualConnection.State.ToString());
            });
        }
    }
}