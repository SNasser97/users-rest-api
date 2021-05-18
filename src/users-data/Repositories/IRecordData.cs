namespace users_data.Repositories
{
    using System;
    using System.Collections.Generic;
    using users_data.Entities;

    public interface IRecordData<TRecord>
        where TRecord : BaseUserRecordWithId
    {
        IDictionary<Guid, BaseUserRecordWithId> Users { get; }
    }
}