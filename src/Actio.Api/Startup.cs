namespace Actio.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.HttpsPolicy;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Actio.Common.Commands;
    using Actio.Common.Events;
    using Actio.Common.Services;
    using Actio.Common.RabbitMq;
    using Actio.Api.Handlers;
    using Actio.Common.Auth;
    using Actio.Api.Repositories;
    using Actio.Common.Mongo;

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
            Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddLogging(builder=>
            {
                builder.AddConfiguration(Configuration.GetSection("Logging"))
                .AddConsole()
                .AddDebug();
            });

            services.AddJwt(Configuration);

            services.AddMongoDb(Configuration);
            
            services.AddRabbitMq(Configuration);

            services.AddTransient<IEventHandler<ActivityCreated>,ActivityCreatedHandler>();
            services.AddTransient<IEventHandler<UserAuthenticated>,UserAuthenticatedHandler>();
            services.AddTransient<IEventHandler<UserCreated>,UserCreatedHandler>();

            services.AddTransient<IActivityRepository,ActivityRepository>();
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

            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}