namespace users_integration_test.Users.Endpoints.GetUsersQuery
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using users_api.UserControllers.QueryControllers.Models.Response;
    using users_integration_test.Common;
    using Xunit;

    public class GetUsersQueryScenario : CommonScenario
    {
        public GetUsersQueryScenario(ApiTestSetup setup) : base(setup)
        {
        }

        public void SetEndpointUrl(string parameter)
        {
            this.Endpoint = parameter;
        }

        protected async override Task OnSubmitRequestAsync(string parameter)
        {
            this.UsersResponse = await this.TestClient.GetAsync(this.Endpoint);
        }

        public async Task ValidateResponseContent()
        {
            string responseContent = await this.UsersResponse.Content.ReadAsStringAsync();
            dynamic s = JsonConvert.DeserializeObject(responseContent);
            var usersResponse = new GetUserControllerResponsesModel
            {
                Users = s.users.ToObject<List<GetUserControllerResponseModel>>()
            };

            Assert.Empty(usersResponse.Users);
        }
    }
}