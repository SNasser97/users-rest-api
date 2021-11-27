namespace users_data.Repositories.MySQL.MySqlDataMapper.Setters
{
    using System;
    using users_data.DataMapper.Setters.Common;

    public class MySqlDataToTypeSetter : BaseTypeSetter
    {
        protected override void OnSetDataToProperty<TEntity>(string typeName, TEntity entity, object data)
        {
            try
            {
                string typeKey = entity.GetType().GetProperty(typeName).PropertyType.Name;
                this.RegisteredTypeSetters[typeKey].SetDataToProperty(typeName, entity, data);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}