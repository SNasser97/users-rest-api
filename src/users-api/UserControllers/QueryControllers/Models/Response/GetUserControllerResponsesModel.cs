namespace users_api.UserControllers.QueryControllers.Models.Response
{
    using System.Collections.Generic;

    public class GetUserControllerResponsesModel
    {
        public IEnumerable<GetUserControllerResponseModel> Users { get; set; }
    }
}