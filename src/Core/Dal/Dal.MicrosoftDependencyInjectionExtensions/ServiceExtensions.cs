using System;
using Dal.Ef.Base;
using Dal.Ef.Contract;
using Dal.Ef.Postgres;
using Dal.Ef.Provider;
using Dex.Ef.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace Dal.MicrosoftDependencyInjectionExtensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDalEfPostgres<TContext, TModelStore>(
            this IServiceCollection services,
            string connectionString,
            Action<NpgsqlDbContextOptionsBuilder> npgsqlOptionsAction = null!)
            where TContext : ResetDbContext
            where TModelStore : class, IModelStore
        {
            services
                .AddSingleton<IModelStore, TModelStore>()
                .AddScoped<IDataProvider, EfDataProvider>()
                .AddScoped<ISafeExecuteProvider, SafeExecuteProvider>()
                .AddScoped<IDataExceptionManager, PostgresDataExceptionManager>()
                .AddEntityFrameworkNpgsql()
                .AddDbContext<TContext>(optionsBuilder =>
                {
                    optionsBuilder.UseNpgsql(connectionString, npgsqlOptionsAction);
                })
                .AddScoped<ResetDbContext>(sp => sp.GetService<TContext>()!);
            return services;
        }
    }
}