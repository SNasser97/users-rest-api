namespace users_integration_test.Common
{
    using System.Net.Http;
    using Microsoft.AspNetCore.Mvc.Testing;
    using users_api;

    public class ApiTestSetup
    {
        public HttpClient Client { get; private set; }

        public string ApiUri { get; private set; }

        private readonly WebApplicationFactory<Startup> appFactory = new WebApplicationFactory<Startup>();

        public ApiTestSetup()
        {
            this.Client = this.appFactory.CreateClient();
            this.ApiUri = Client?.BaseAddress?.AbsoluteUri;
        }
    }
}