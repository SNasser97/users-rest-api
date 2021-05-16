namespace users_logic.User.Logic.Query.Models.Response
{
    using System.Collections.Generic;

    public class GetUsersQueryResponseModel
    {
        public IEnumerable<GetUserQueryResponseModel> Users { get; set; }
    }
}
