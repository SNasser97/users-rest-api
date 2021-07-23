namespace users_logic.User.Logic.Query.Models.Response
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public class GetUsersQueryResponseModel
    {
        public IEnumerable<GetUserQueryResponseModel> Users { get => this.users; }

        private readonly ConcurrentBag<GetUserQueryResponseModel> users = new ConcurrentBag<GetUserQueryResponseModel>();

        public void AddResponse(GetUserQueryResponseModel model)
        {
            this.users.Add(model);
        }
    }
}
