namespace users_data.Manager
{
    using System.Data;
    using System.Data.Common;
    using System.Threading.Tasks;

    public interface IDbConnectionManager<TConnection>
        where TConnection : IDbConnection
    {
        Task<TConnection> GetDbConnectionAsync();

        // TODO : remove, using(conn) {..} closes after using
        Task CloseDbConnectionAsync(TConnection connection);

    }

    public interface IDbConnectionManager : IDbConnectionManager<DbConnection>
    { }
}