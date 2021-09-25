using System.Threading.Tasks;

namespace users_data.Manager
{
    public interface IDbTransactionManager
    {
        Task ExecuteTransactionAsync();
    }
}