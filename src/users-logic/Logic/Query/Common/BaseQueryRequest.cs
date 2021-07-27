using System.Threading.Tasks;
using users_data.Repositories;

namespace users_logic.Logic.Query.Common
{
    public abstract class BaseQueryRequest<TQueryRequest, TQueryResponse, TRecord> : IQuery<TQueryRequest, TQueryResponse>
    {
        protected readonly IReadRepository<TRecord> readRepository;

        public BaseQueryRequest(IReadRepository<TRecord> readRepository)
        {
            this.readRepository = readRepository;
        }

        public async Task<TQueryResponse> ExecuteAsync(TQueryRequest request)
            => await this.OnExecuteAsync(request);

        protected abstract Task<TQueryResponse> OnExecuteAsync(TQueryRequest request);
    }
}