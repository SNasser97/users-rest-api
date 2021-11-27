namespace users_data.DataMapper.Setters
{
    using System;
    using System.Reflection;
    using users_data.DataMapper.Setters.Common;

    public class DateTimeSetter : ITypeSetter
    {
        public void SetDataToProperty<TEntity>(string propertyName, TEntity entity, object data)
        {
            PropertyInfo propertyInfo = entity.GetType().GetProperty(propertyName);
            propertyInfo.SetValue(entity, DateTime.Parse(data.ToString()));
        }
    }
}