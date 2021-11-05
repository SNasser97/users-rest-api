namespace users_integration_test.Users.Endpoints.GetUsersQuery
{
    using System.Threading.Tasks;
    using TechTalk.SpecFlow;
    using users_integration_test.Common;

    [Binding]
    [Scope(Feature = "Users Query")]
    public class GetUsersQuerySteps : CommonStep<GetUsersQueryScenario>
    {
        public GetUsersQuerySteps(GetUsersQueryScenario scenario) : base(scenario)
        {
        }

        [When("a request is made to the endpoint (.*)")]
        public async Task WhenARequestIsMadeToGetUsers(string parameter)
        {
            await this.scenario.SubmitRequestAsync(parameter);
        }

        [Then("an empty array is returned")]
        public async Task CheckContent()
        {
            await this.scenario.ValidateResponseContent();
        }
    }
}