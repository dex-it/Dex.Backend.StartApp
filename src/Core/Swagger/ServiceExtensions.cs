using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Swagger
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddSwagger(
            this IServiceCollection services,
            ApiVersion defaultApiVersion, 
            string title)
        {
            services.AddApiVersioning(o =>
                {
                    o.ReportApiVersions = true;
                    o.AssumeDefaultVersionWhenUnspecified = true;
                    o.DefaultApiVersion = defaultApiVersion;
                })
                .AddVersionedApiExplorer(o =>
                {
                    o.GroupNameFormat = "'v'VVV";
                    o.SubstituteApiVersionInUrl = true;
                })
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = title,
                        Version = defaultApiVersion.ToString()
                    });
                });
            return services;
        }
    }
}