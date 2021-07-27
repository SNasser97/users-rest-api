namespace users_data.Repositories.InMemoryUserRepository
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using users_data.Entities;

    // Where InMemory of entity written to and read from
    public class InMemoryUsersRecordData : IRecordData<User>
    {
        public IDictionary<Guid, User> EntityStorage { get => this.users; }

        private readonly IDictionary<Guid, User> users = new ConcurrentDictionary<Guid, User>();
    }
}