using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Server.Bll.MicrosoftDependencyInjectionExtensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddBll(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services;
        }
    }
}