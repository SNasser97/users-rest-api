namespace users_data.DataMapper.Setters.Common
{
    using System;
    using System.Collections.Generic;

    public abstract class BaseTypeSetter : ITypeSetter
    {
        // Register data types for a given prop data types in an entity here.
        protected IDictionary<string, ITypeSetter> RegisteredTypeSetters => new Dictionary<string, ITypeSetter>
        {
            { nameof(DateTime), new DateTimeSetter() },
            { nameof(Int32), new Int32Setter() },
            { nameof(String), new StringSetter() },
            { nameof(Guid), new GuidSetter() }
        };

        public void SetDataToProperty<TEntity>(string typeName, TEntity entity, object data)
        {
            this.OnSetDataToProperty<TEntity>(typeName, entity, data);
        }

        protected abstract void OnSetDataToProperty<TEntity>(string typeName, TEntity entity, object data);
    }
}