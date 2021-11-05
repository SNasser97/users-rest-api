namespace users_integration_test.Common
{
    using System.Net;
    using TechTalk.SpecFlow;
    using Xunit;

    public abstract class CommonStep<TScenario>
        where TScenario : CommonScenario
    {
        protected readonly TScenario scenario;

        public CommonStep(TScenario scenario)
        {
            this.scenario = scenario;
        }

        [Given("query made to endpoint with (.*)")]
        public void SetupEndpoint(string endpoint)
        {
            this.scenario.Endpoint = endpoint;
        }

        [Then("a 200 OK is returned")]
        public void Then200ResponseReturned()
        {
            Assert.Equal(HttpStatusCode.OK, this.scenario.UsersResponse.StatusCode);
        }
    }
}