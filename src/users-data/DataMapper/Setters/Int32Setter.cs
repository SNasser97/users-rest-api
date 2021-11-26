namespace users_data.DataMapper.Setters
{
    using System;
    using System.Reflection;
    using users_data.DataMapper.Setters.Common;

    public class Int32Setter : ITypeSetter
    {
        public void SetDataToProperty<TEntity>(string typeName, TEntity entity, object data)
        {
            PropertyInfo propertyInfo = entity.GetType().GetProperty(typeName);
            propertyInfo.SetValue(entity, Convert.ToInt32(data));
        }
    }
}