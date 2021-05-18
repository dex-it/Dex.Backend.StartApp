using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Dal.MicrosoftDependencyInjectionExtensions;
using Server.Dal.EF;
using Server.Dal.EF.Provider;

namespace Server.Dal.MicrosoftDependencyInjectionExtensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDal(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDalEfPostgres<ServerDbContext, ModelStore>(configuration["DbConnection"]);
            
            return services;
        }
    }
}