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

    public class UserQuery : IUserQuery<GetUserQueryRequestModel, GetUserQueryResponseModel>
    {
        private readonly IReadRepository<User> userReadRepository;

        public UserQuery(IReadRepository<User> userReadRepository)
        {
            this.userReadRepository = userReadRepository ?? throw new System.ArgumentNullException(nameof(userReadRepository));
        }

        public async Task<GetUserQueryResponseModel> GetResponseAsync(GetUserQueryRequestModel request)
        {
            UserLogic.ThrowException<ArgumentNullException>(() => request == null, nameof(request));
            UserLogic.ThrowException<QueryRequestException>(() => request.Id == Guid.Empty);

            User userRecord = await this.userReadRepository.GetAsync(request.Id);

            UserLogic.ThrowException<UserNotFoundException>(() => userRecord == null);
            return userRecord.ToResponseModel();
        }

        public async Task<IEnumerable<GetUserQueryResponseModel>> GetResponsesAsync()
        {
            IEnumerable<User> userRecords = await this.userReadRepository.GetAsync();
            return await this.MapToGetUsersQueryResponseModel(userRecords);
        }

        private async Task<IEnumerable<GetUserQueryResponseModel>> MapToGetUsersQueryResponseModel(IEnumerable<User> records)
        {
            // IList<GetUserQueryResponseModel> responses = await (Task.WhenAll(records.Select(r
            // => Task.Run(() => r.ToResponseModel()))))
            // var coll = new List<Task<GetUserQueryResponseModel>>();

            // records.ForEach
            // var usersQueryResponseModel = new GetUsersQueryResponseModel();

            // responses.ForEachResponse(async r => await Task.Run(() => usersQueryResponseModel.AddResponse(r)));
            return await (Task.WhenAll(records.Select(r
                => Task.Run(() => r.ToResponseModel()))));
        }
    }
}