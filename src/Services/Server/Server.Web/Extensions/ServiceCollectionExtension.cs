using Dex.Ef.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.Dal;
using Server.Dal.Base;
using Server.Dal.Contract;
using Server.Dal.Provider;

namespace Server.Web.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDal(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<IModelStore, ModelStore>()
                .AddScoped<IDataProvider, EfDataProvider>()
                .AddScoped<ISafeExecuteProvider, SafeExecuteProvider>()
                .AddScoped<IDataExceptionManager, PostgresDataExceptionManager>();

            services
                .AddEntityFrameworkNpgsql()
                .AddDbContext<ServerEfDataConnection>(optionsBuilder =>
                {
                    optionsBuilder.UseNpgsql(configuration["DbConnection"]);
                })
                .AddScoped<ResetDbContext>(sp => sp.GetService<ServerEfDataConnection>());

            return services;
        }
    }
}