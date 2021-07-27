using System;
using System.Threading.Tasks;
using users_data.Entities;
using users_data.Repositories;
using users_logic.Exceptions.Query;
using users_logic.Exceptions.UserExceptions;
using users_logic.Extensions;
using users_logic.Logic.Query.Common;

namespace users_logic.Logic.Query.GetUserQuery
{
    public class GetUserQuery : BaseQueryRequest<GetUserQueryRequest, GetUserQueryResponse, User>, IGetUserQuery
    {
        public GetUserQuery(IReadRepository<User> readRepository) : base(readRepository)
        {
        }

        protected override async Task<GetUserQueryResponse> OnExecuteAsync(GetUserQueryRequest request)
        {
            UserLogic.ThrowException<ArgumentNullException>(() => request == null, nameof(request));
            UserLogic.ThrowException<QueryRequestException>(() => request.Id == Guid.Empty);

            User userRecord = await this.readRepository.GetAsync(request.Id);

            UserLogic.ThrowException<UserNotFoundException>(() => userRecord == null);
            return userRecord.ToResponseModel();
        }
    }
}