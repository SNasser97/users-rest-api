namespace users_data.Entities
{
    using System;

    public class UserRecord : BaseUserRecord, IUserRecordId
    {
        public Guid Id { get; set; }
    }
}
