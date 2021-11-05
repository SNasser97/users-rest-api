namespace users_integration_test.Common
{
    using System.Net.Http;
    using System.Threading.Tasks;

    public abstract class CommonScenario
    {
        public string Endpoint { get; set; }
        public HttpResponseMessage UsersResponse { get; set; }

        protected HttpClient TestClient { get; private set; }

        private readonly ApiTestSetup setup;

        public CommonScenario(ApiTestSetup setup)
        {
            this.setup = setup;
            this.TestClient = setup?.Client;
        }

        public async Task SubmitRequestAsync(string parameter)
        {
            await this.OnSubmitRequestAsync(parameter);
        }

        protected abstract Task OnSubmitRequestAsync(string parameter);
    }
}