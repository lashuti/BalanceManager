using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace BalanceManager.API.StartupConfiguration
{
    public static class ConfigureSwagger
    {
        public static void ConfigureSwaggerOptions(IServiceCollection services)
        {
            services.AddSwaggerGenNewtonsoftSupport();

            string _title = "BalanceManager API";
            string _description = "API For Managing Casino & Game Balance";

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = _title,
                    Version = "v1",
                    Description = _description,
                });

                c.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = _title,
                    Version = "v2",
                    Description = _description,
                });

            });

            services.AddApiVersioning(cfg =>
            {
                cfg.DefaultApiVersion = new ApiVersion(1, 0);
                cfg.AssumeDefaultVersionWhenUnspecified = true;
                cfg.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddRouting(options => options.LowercaseUrls = true);
        }
    }
}
