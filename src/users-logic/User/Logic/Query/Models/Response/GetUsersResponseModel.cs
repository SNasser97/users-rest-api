namespace users_logic.User.Logic.Query.Models.Response
{
    using System.Collections.Generic;

    public class GetUsersResponseModel
    {
        public IEnumerable<GetUserResponseModel> Users { get; set; }
    }
}
