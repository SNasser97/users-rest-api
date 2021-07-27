namespace users_data.Repositories
{
    using System;
    using System.Collections.Generic;

    public interface IRecordData<TRecord>
    {
        IDictionary<Guid, TRecord> EntityStorage { get; }
    }
}