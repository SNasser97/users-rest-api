namespace users_data.DataMapper.Setters.Common
{
    public interface ITypeSetter
    {
        void SetDataToProperty<TEntity>(string typeName, TEntity entity, object data);
    }
}