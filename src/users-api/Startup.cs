using System;
using System.Collections.Generic;
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

namespace users_api
{
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
            services.AddScoped<IUserQuery<GetUserQueryRequestModel, GetUserQueryResponseModel, GetUsersQueryResponseModel>, UserQuery>();
            services.AddScoped<IUserCommand<BaseUserCommandResponse>, UserCommand>();
            services.AddScoped<IDateTimeParser, DateTimeParser>();
            services.AddScoped<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<IUserLogicFacade, UserLogicFacade>();
            services.AddSingleton<InMemoryUsersRepository>();
            services.AddSingleton<IWriteRepository<BaseUserRecord, BaseUserRecordWithId>>(x => x.GetRequiredService<InMemoryUsersRepository>());
            services.AddSingleton<IReadRepository<BaseUserRecordWithId>>(x => x.GetRequiredService<InMemoryUsersRepository>());
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
    }
}
