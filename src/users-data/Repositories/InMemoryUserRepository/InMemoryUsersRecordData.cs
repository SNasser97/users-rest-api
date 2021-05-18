namespace users_data.Repositories.InMemoryUserRepository
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using users_data.Entities;

    public class InMemoryUsersRecordData : IRecordData<BaseUserRecordWithId>
    {
        public IDictionary<Guid, BaseUserRecordWithId> Users { get => this.users; }

        private readonly IDictionary<Guid, BaseUserRecordWithId> users = new ConcurrentDictionary<Guid, BaseUserRecordWithId>();
    }
}