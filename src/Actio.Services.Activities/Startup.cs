using Actio.Common.Commands;
using Actio.Common.Mongo;
using Actio.Common.RabbitMq;
using Actio.Services.Activities.Domain.Repositories;
using Actio.Services.Activities.Handlers;
using Actio.Services.Activities.Repositories;
using Actio.Services.Activities.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Actio.Services.Activities
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddLogging(builder =>
            {
                builder.AddConfiguration(Configuration.GetSection("Logging"))
                .AddConsole()
                .AddDebug();
            });

            services.AddMongoDb(Configuration);

            services.AddRabbitMq(Configuration);

            services.AddTransient<ICommandHandler<CreateActivity>, CreateActivityHandler>();

            services.AddTransient<IActivityRepository, ActivityRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();

            services.AddTransient<IDatabaseSeeder, ActivityMongoSeeder>();

            services.AddTransient<IActivityService, ActivityService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var seeder = app.ApplicationServices.GetService<IDatabaseSeeder>();
            app.ApplicationServices.GetService<IDatabaseInitializer>().InitializeAsync(seeder);

            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
