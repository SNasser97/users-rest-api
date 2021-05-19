namespace users_api
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using users_data.Entities;
    using users_data.Repositories;
    using users_data.Repositories.InMemoryUserRepository;
    using users_logic.User.Facades;
    using users_logic.User.Logic.Command;
    using users_logic.User.Logic.Command.Models.Response;
    using users_logic.User.Logic.Query;
    using users_logic.User.Logic.Query.Models.Request;
    using users_logic.User.Logic.Query.Models.Response;
    using users_logic.User.Parser;
    using users_logic.User.Provider;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            this.AddUserDependencies(services);
            services.AddControllers();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void AddUserDependencies(IServiceCollection services)
        {
            // Where IRecordData contains user data which user write + read access
            services.AddSingleton<IRecordData<BaseUserRecordWithId>, InMemoryUsersRecordData>();
            services.AddScoped<IWriteRepository<BaseUserRecord, BaseUserRecordWithId>, InMemoryUserWriteRepository>();
            services.AddScoped<IReadRepository<BaseUserRecordWithId>, InMemoryUserReadRepository>();
            services.AddScoped<IUserQuery<GetUserQueryRequestModel, GetUserQueryResponseModel, GetUsersQueryResponseModel>, UserQuery>();
            services.AddScoped<IUserCommand<BaseUserCommandResponseModel>, UserCommand>();
            services.AddScoped<IDateTimeParser, DateTimeParser>();
            services.AddScoped<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<IUserLogicFacade, UserLogicFacade>();
        }
    }
}
