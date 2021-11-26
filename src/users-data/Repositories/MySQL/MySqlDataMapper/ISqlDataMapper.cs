namespace users_data.Repositories.MySQL.MySqlDataMapper
{
    using System.Data;
    using users_data.DataMapper;

    public interface ISqlDataMapper<TEntity> : IDataMapper<TEntity, IDataReader>
    {
    }
}