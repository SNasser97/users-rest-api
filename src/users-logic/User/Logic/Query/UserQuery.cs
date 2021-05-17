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
        private readonly IReadRepository<UserRecord> userReadRepository;

        public UserQuery(IReadRepository<UserRecord> userReadRepository)
        {
            this.userReadRepository = userReadRepository ?? throw new System.ArgumentNullException(nameof(userReadRepository));
        }

        public async Task<GetUserQueryResponseModel> GetReponseAsync(GetUserQueryRequestModel request)
        {
            ExecuteLogic.ThrowException<ArgumentNullException>(() => request == null, nameof(request));
            ExecuteLogic.ThrowException<QueryRequestException>(() => request.Id == Guid.Empty);

            UserRecord userRecord = await this.userReadRepository.GetAsync(request.Id);

            ExecuteLogic.ThrowException<UserNotFoundException>(() => userRecord == null);
            return new GetUserQueryResponseModel().ToUserResponseModel(userRecord);
        }

        public async Task<GetUsersQueryResponseModel> GetReponsesAsync()
        {
            IEnumerable<UserRecord> userRecords = await this.userReadRepository.GetAsync();
            return await this.MapToGetUsersQueryResponseModel(userRecords);
        }

        private async Task<GetUsersQueryResponseModel> MapToGetUsersQueryResponseModel(IEnumerable<UserRecord> records)
        {
            IEnumerable<GetUserQueryResponseModel> userQueryResponseList = await (Task.WhenAll(records.Select(r
                => Task.Run(() => new GetUserQueryResponseModel().ToUserResponseModel(r)))));

            var usersQueryResponse = new GetUsersQueryResponseModel();
            usersQueryResponse.Users = userQueryResponseList;

            return usersQueryResponse;
        }
    }
}