namespace users_data.DataMapper.Setters
{
    using System;
    using System.Reflection;
    using users_data.DataMapper.Setters.Common;

    public class GuidSetter : ITypeSetter
    {
        public void SetDataToProperty<TEntity>(string typeName, TEntity entity, object data)
        {
            PropertyInfo propertyInfo = entity.GetType().GetProperty(typeName);
            propertyInfo.SetValue(entity, new Guid(data as byte[]));
        }
    }
}