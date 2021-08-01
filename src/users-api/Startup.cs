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
    using users_logic.Facades;
    using users_logic.Logic.Command.CreateUserCommand;
    using users_logic.Logic.Command.DeleteUserCommand;
    using users_logic.Logic.Command.UpdateUserCommand;
    using users_logic.Logic.Query.GetUserQuery;
    using users_logic.Logic.Query.GetUsersQuery;
    using users_logic.Parser;
    using users_logic.Provider;

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

            // app.UseHttpsRedirection();

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
            services.AddSingleton<IRecordData<User>, InMemoryUsersRecordData>();
            services.AddScoped<IWriteRepository<User>, InMemoryUserWriteRepository>();
            services.AddScoped<IReadRepository<User>, InMemoryUserReadRepository>();
            services.AddScoped<ICreateUserCommand, CreateUserCommand>();
            services.AddScoped<IUpdateUserCommand, UpdateUserCommand>();
            services.AddScoped<IDeleteUserCommand, DeleteUserCommand>();
            services.AddScoped<IGetUserQuery, GetUserQuery>();
            services.AddScoped<IGetUsersQuery, GetUsersQuery>();
            services.AddScoped<IDateTimeParser, DateTimeParser>();
            services.AddScoped<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<IUserLogicFacade, UserLogicFacade>();
        }
    }
}
