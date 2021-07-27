using System.Collections.Generic;

namespace users_logic.Logic.Query.GetUsersQuery
{
    public interface IGetUsersQuery : IQuery<GetUserQueryRequest, IEnumerable<GetUserQueryResponse>>
    {
    }
}