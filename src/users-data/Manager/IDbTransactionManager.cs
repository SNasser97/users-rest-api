namespace users_data.Manager
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Threading.Tasks;

    public interface IDbTransactionManager<TDbConnection>
    {
        Task<TValue> ExecuteTransactionAsync<TValue>(Func<IDbTransaction, Task<TValue>> transactionFunc);
    }

    public interface IDbTransactionManager : IDbTransactionManager<IDbTransaction>
    { }
}