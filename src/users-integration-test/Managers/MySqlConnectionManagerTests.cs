namespace users_integration_test.Managers.Managers
{
    using System;
    using System.Data.Common;
    using System.Threading.Tasks;
    using MySql.Data.MySqlClient;
    using users_data.Repositories.MySQL.MySqlManagers;
    using Xunit;
    using users_test.Helper;

    public class MySqlConnectionManagerTests
    {
        // Requires a running mysql docker container for tests
        private string connectionErrorMessage = "Access denied for user";
        private string noMySqlConnectionMessage = "Unable to connect to any of the specified MySQL hosts.";
        private Lazy<string> connectionStringTestValue = new Lazy<string>(Environment.GetEnvironmentVariable("MYSQL_CONNECTION") ?? $"Server=localhost;Uid=admin;Pwd=secret;Database=users_db;");

        [SkippableFact]
        public async Task MySqlConnectionManagerGetDbConnectionAsyncReturnsInvalidMySqlConnectionEntity()
        {
            // Given

            var connectionString = new Lazy<string>("");
            var connectionManager = new MySqlConnectionManager(connectionString);

            // When
            MySqlConnection actualConnection = (MySqlConnection)await connectionManager.GetDbConnectionAsync();
            await this.SkipTestOnCondition<MySqlConnection, MySqlException>(actualConnection, noMySqlConnectionMessage);

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
            // Assert.True(ex.Message.Contains(connectionErrorMessage))
        }

        [SkippableFact]
        public async Task MySqlConnectionManagerGetDbConnectionAsyncReturnsValidMySqlConnectionEntity()
        {
            // Given
            var connectionManager = new MySqlConnectionManager(connectionStringTestValue);

            // When
            MySqlConnection actualConnection = (MySqlConnection)await connectionManager.GetDbConnectionAsync();
            await this.SkipTestOnCondition<MySqlConnection, MySqlException>(actualConnection, noMySqlConnectionMessage);

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

        // TODO: Extract as helper methods
        private async Task<bool> ExceptionMessageOn<TConnectionEntity, TException>(TConnectionEntity entity, string message)
            where TConnectionEntity : DbConnection
            where TException : Exception
        {
            try
            {
                await entity.OpenAsync();
                await entity.CloseAsync();
                return false;
            }
            catch (TException ex)
            {
                return ex.Message.Equals(message);
            }
        }

        private async Task SkipTestOnCondition<TConnectionEntity, TException>(TConnectionEntity entity, string message)
            where TConnectionEntity : DbConnection
            where TException : Exception
        {
            // Ignore the above tests if we're building the dotnet api image
            bool isDotNetImageBeingBuilt = await this.ExceptionMessageOn<TConnectionEntity, TException>(entity, message);
            Skip.If(isDotNetImageBeingBuilt, "Only run on dev machine");
        }
    }
}