using System.IO;
using Dex.Ef.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Server.Dal.EF;
using Server.Dal.EF.Provider;

namespace Server.Dal.Ef.Migrator
{
    internal class DbContextFactory : IDesignTimeDbContextFactory<DbContextFactory.MigratorDbContext>
    {
        public MigratorDbContext CreateDbContext(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Server.Migrator.appsettings.json", true, true)
                .AddJsonFile($"Server.Migrator.appsettings.{ServerMigratorProgram.EnvironmentName}.json", true)
                .AddJsonFile("Server.Migrator.appsettings.local.json", true);

            var configuration = configurationBuilder.Build();

            var modelStore = new ModelStore();

            ILoggerFactory loggerFactory = new LoggerFactory();

            var dbContextOptions = new DbContextOptionsBuilder()
                .UseNpgsql(configuration["DbConnection"], 
                    o => o.MigrationsAssembly(GetType().Namespace)
                        .MigrationsHistoryTable(HistoryRepository.DefaultTableName, "service"))
                .Options;

            return new MigratorDbContext(modelStore, dbContextOptions, loggerFactory);
        }

        public class MigratorDbContext : ServerDbContext
        {
            private readonly ILoggerFactory _loggerFactory;

            public MigratorDbContext(
                IModelStore modelStore,
                DbContextOptions options,
                ILoggerFactory loggerFactory)
                : base(modelStore, options)
            {
                _loggerFactory = loggerFactory;
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                base.OnConfiguring(optionsBuilder);

                if (_loggerFactory != null)
                {
                    optionsBuilder.UseLoggerFactory(_loggerFactory);
                }
            }

            protected override void OnModelCreating(ModelBuilder builder)
            {
                base.OnModelCreating(builder);
                
                DataSeedingProvider.Config(builder);
            }
        }
    }
}