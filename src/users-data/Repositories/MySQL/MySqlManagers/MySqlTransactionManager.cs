namespace users_data.Repositories.MySQL.MySqlManagers
{
    using System;
    using System.Data;
    using System.Threading.Tasks;
    using MySql.Data.MySqlClient;
    using users_data.Manager;

    public class MySqlTransactionManager : IDbTransactionManager
    {
        private readonly IDbConnectionManager connectionManager;

        public MySqlTransactionManager(IDbConnectionManager connectionManager)
        {
            this.connectionManager = connectionManager;
        }
        public async Task<TValue> ExecuteTransactionAsync<TValue>(Func<IDbTransaction, Task<TValue>> transactionFunc)
        {
            using (MySqlConnection connection = await connectionManager.GetDbConnectionAsync() as MySqlConnection)
            {
                await connection.OpenAsync();
                MySqlTransaction transaction = await connection.BeginTransactionAsync();
                try
                {
                    TValue result = await transactionFunc(transaction);
                    await transaction.CommitAsync();
                    return result;
                }
                catch (MySqlException ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    await connection.CloseAsync();
                }
            }
        }
    }
}