namespace users_data.Repositories.MySQL.MySqlDataMapper
{
    using System.Data;
    using users_data.DataMapper.Setters.Common;

    public class MySqlDataMapper<TEntity> : ISqlDataMapper<TEntity>
        where TEntity : class, new()
    {
        private readonly ITypeSetter typeSetter;

        public MySqlDataMapper(ITypeSetter typeSetter)
        {
            this.typeSetter = typeSetter;
        }

        public TEntity MapDataToEntity(IDataReader reader)
        {
            var entity = new TEntity();

            for (int currentOrdinal = 0; currentOrdinal < reader.FieldCount; currentOrdinal++)
            {
                object rowValue = reader.GetValue(currentOrdinal);
                string typeName = reader.GetName(currentOrdinal);
                this.typeSetter.SetDataToProperty<TEntity>(typeName, entity, rowValue);
            }

            return entity;
        }
    }
}