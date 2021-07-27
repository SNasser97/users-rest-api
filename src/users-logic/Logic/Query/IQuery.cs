using System.Threading.Tasks;

namespace users_logic.Logic.Query
{
    public interface IQuery<TQueryRequest, TQueryResponse>
    {
        Task<TQueryResponse> ExecuteAsync(TQueryRequest request);
    }
}