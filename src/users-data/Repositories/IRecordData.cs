namespace users_data.Repositories
{
    using System;
    using System.Collections.Generic;
    using users_data.Entities;

    public interface IRecordData<TRecord>
    {
        IDictionary<Guid, TRecord> EntityStorage { get; }
    }
}