using System.Collections.Generic;
using System.Threading.Tasks;
using users_data.Entities;
using users_data.Repositories;
using users_logic.Extensions;
using users_logic.Logic.Query.Common;

namespace users_logic.Logic.Query.GetUsersQuery
{
    public class GetUsersQuery : BaseQueryRequest<GetUserQueryRequest, IEnumerable<GetUserQueryResponse>, User>, IGetUsersQuery
    {
        public GetUsersQuery(IReadRepository<User> readRepository) : base(readRepository)
        {
        }

        protected override async Task<IEnumerable<GetUserQueryResponse>> OnExecuteAsync(GetUserQueryRequest request)
        {
            IEnumerable<User> userRecords = await this.readRepository.GetAsync();
            return await userRecords.MapToGetUsersQueryResponseModel();
        }
    }
}