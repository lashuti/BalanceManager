using BalanceManager.Application.Abstractions;
using BalanceManager.Application.Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Converters;
using BalanceManager.API.StartupConfiguration;

namespace BalanceManager
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.Converters.Add(new StringEnumConverter()));

            ConfigureSwagger.ConfigureSwaggerOptions(services);

            services.AddRouting(options => options.LowercaseUrls = true);
            
            services.AddTransient<IBalanceService, BalanceService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Exception description and stack trace won't get displayed in Swagger but in real project it's details would be logged
            app.ConfigureExceptionHandler(); 

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BalanceManager API V1");
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "BalanceManager API V2");
            });

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
