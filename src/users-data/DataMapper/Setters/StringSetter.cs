namespace users_data.DataMapper.Setters
{
    using System.Reflection;
    using users_data.DataMapper.Setters.Common;

    public class StringSetter : ITypeSetter
    {
        public void SetDataToProperty<TEntity>(string typeName, TEntity entity, object data)
        {
            PropertyInfo propertyInfo = entity.GetType().GetProperty(typeName);
            propertyInfo.SetValue(entity, data);
        }
    }
}