namespace users_data.Entities
{
    using System;

    public class UpdateUserRecord : BaseUserRecord, IUserRecordId
    {
        public Guid Id { get; set; }
    }
}