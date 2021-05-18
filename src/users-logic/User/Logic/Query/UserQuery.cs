namespace users_logic.User.Logic.Query
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using users_data.Entities;
    using users_data.Repositories;
    using users_logic.Exceptions.Query;
    using users_logic.Exceptions.User;
    using users_logic.Extensions;
    using users_logic.User.Logic.Query.Models.Request;
    using users_logic.User.Logic.Query.Models.Response;

    public class UserQuery : IUserQuery<GetUserQueryRequestModel, GetUserQueryResponseModel, GetUsersQueryResponseModel>
    {
        private readonly IReadRepository<BaseUserRecordWithId> userReadRepository;

        public UserQuery(IReadRepository<BaseUserRecordWithId> userReadRepository)
        {
            this.userReadRepository = userReadRepository ?? throw new System.ArgumentNullException(nameof(userReadRepository));
        }

        public async Task<GetUserQueryResponseModel> GetReponseAsync(GetUserQueryRequestModel request)
        {
            ExecuteLogic.ThrowException<ArgumentNullException>(() => request == null, nameof(request));
            ExecuteLogic.ThrowException<QueryRequestException>(() => request.Id == Guid.Empty);

            BaseUserRecordWithId userRecord = await this.userReadRepository.GetAsync(request.Id);

            ExecuteLogic.ThrowException<UserNotFoundException>(() => userRecord == null);
            return new GetUserQueryResponseModel().ToUserResponseModel(userRecord);
        }

        public async Task<GetUsersQueryResponseModel> GetResponsesAsync()
        {
            IEnumerable<BaseUserRecordWithId> userRecords = await this.userReadRepository.GetAsync();
            return await this.MapToGetUsersQueryResponseModel(userRecords);
        }

        private async Task<GetUsersQueryResponseModel> MapToGetUsersQueryResponseModel(IEnumerable<BaseUserRecordWithId> records)
        {
            IEnumerable<GetUserQueryResponseModel> userQueryResponseList = await (Task.WhenAll(records.Select(r
                => Task.Run(() => new GetUserQueryResponseModel().ToUserResponseModel(r)))));

            var usersQueryResponse = new GetUsersQueryResponseModel();
            usersQueryResponse.Users = userQueryResponseList;
            return usersQueryResponse;
        }
    }
}