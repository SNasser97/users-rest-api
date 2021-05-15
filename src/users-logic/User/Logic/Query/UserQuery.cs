namespace users_logic.User.Logic.Query
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using users_data.Repositories;
    using users_logic.User.Logic.Query.Models.Request;
    using users_logic.User.Logic.Query.Models.Response;

    public class UserQuery : IUserQuery<GetUserRequestModel, GetUserResponseModel>
    {
        private readonly IReadRepository<GetUserResponseModel> userReadRepository;

        public UserQuery(IReadRepository<GetUserResponseModel> userReadRepository)
        {
            this.userReadRepository = userReadRepository ?? throw new System.ArgumentNullException(nameof(userReadRepository));
        }

        public Task<GetUserResponseModel> GetReponseAsync(GetUserRequestModel request)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<GetUserResponseModel>> GetReponsesAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}