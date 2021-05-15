namespace users_logic.User.Logic.Query
{
    using System;

    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using users_data.Entities;
    using users_data.Repositories;
    using users_logic.Exceptions.Command;
    using users_logic.Exceptions.User;
    using users_logic.User.Logic.Query.Models.Request;
    using users_logic.User.Logic.Query.Models.Response;

    public class UserQuery : IUserQuery<GetUserRequestModel, GetUserResponseModel>
    {
        private readonly IReadRepository<UserRecord> userReadRepository;

        public UserQuery(IReadRepository<UserRecord> userReadRepository)
        {
            this.userReadRepository = userReadRepository ?? throw new System.ArgumentNullException(nameof(userReadRepository));
        }

        public async Task<GetUserResponseModel> GetReponseAsync(GetUserRequestModel request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Id == Guid.Empty)
            {
                throw new QueryRequestException("Request Id cannot be empty");
            }

            UserRecord userRecord = await this.userReadRepository.GetAsync(request.Id);

            if (userRecord == null)
            {
                throw new UserNotFoundException("user not found");
            }

            return new GetUserResponseModel().ToUserResponseModel(userRecord);
        }

        public async Task<IEnumerable<GetUserResponseModel>> GetReponsesAsync()
        {
            IEnumerable<UserRecord> userRecords = await this.userReadRepository.GetAsync();

            var userResponses = new List<Task<GetUserResponseModel>>();

            foreach (UserRecord userRecord in userRecords)
            {
                Task<GetUserResponseModel> userResponse = Task.Run(() => new GetUserResponseModel().ToUserResponseModel(userRecord));
                userResponses.Add(userResponse);
            }

            return await Task.WhenAll(userResponses);
        }
    }
}