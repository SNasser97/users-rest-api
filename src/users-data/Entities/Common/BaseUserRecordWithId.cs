namespace users_data.Entities
{
    using System;

    public abstract class BaseUserRecordWithId : BaseUserRecord
    {
        public Guid Id { get; set; }
    }
}