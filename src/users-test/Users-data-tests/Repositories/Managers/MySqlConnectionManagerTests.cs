namespace users_test.Users_data_tests.Repositories.Managers
{
    using System;
    using System.Threading.Tasks;
    using MySql.Data.MySqlClient;
    using users_data.Repositories.MySQL.MySqlManagers;
    using Xunit;

    public class MySqlConnectionManagerTests
    {
        // Requires a running mysql docker container for tests
        private string connectionErrorMessage = "Access denied for user ''@'172.18.0.1' (using password: NO)";
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
            }, (ex) => Assert.True(ex.Message.Contains(connectionErrorMessage)));
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