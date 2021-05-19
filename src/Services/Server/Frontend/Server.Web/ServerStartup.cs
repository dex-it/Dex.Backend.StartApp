using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Server.Bll.MicrosoftDependencyInjectionExtensions;
using Server.Dal.MicrosoftDependencyInjectionExtensions;
using Swagger;

namespace Server.Web
{
    public class ServerStartup
    {
        private const string AllowOrigin = "AllowSpecificOrigin";

        private readonly IConfiguration _configuration;

        public ServerStartup(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors(o => o.AddPolicy(AllowOrigin,
                b => b.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod())
            );
            services.AddBll(_configuration);
            services.AddDal(_configuration);
            services.AddSwagger(new ApiVersion(1, 0), "Server API");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env,
            IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger(provider);
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(AllowOrigin);

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}