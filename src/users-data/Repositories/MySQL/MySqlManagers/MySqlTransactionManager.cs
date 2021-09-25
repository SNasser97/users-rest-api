namespace users_data.Repositories.MySQL.MySqlManagers
{
    using System.Threading.Tasks;
    using users_data.Manager;

    public class MySqlTransactionManager : IDbTransactionManager
    {
        public Task ExecuteTransactionAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}