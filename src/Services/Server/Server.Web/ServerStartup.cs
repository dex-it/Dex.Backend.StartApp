using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Server.Web.Extensions;
using Swagger;

namespace Server.Web
{
    public class ServerStartup
    {
        private const string AllowOrigin = "AllowSpecificOrigin";

        private IConfiguration Configuration { get; }

        public ServerStartup(
            IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors(o => o.AddPolicy(AllowOrigin,
                b => b.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod())
            );
            services.AddDal(Configuration);
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