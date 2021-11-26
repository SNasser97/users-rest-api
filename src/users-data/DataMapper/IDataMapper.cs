namespace users_data.DataMapper
{
    public interface IDataMapper<TEntity, TDataReader>
    {
        TEntity MapDataToEntity(TDataReader reader);
    }
}