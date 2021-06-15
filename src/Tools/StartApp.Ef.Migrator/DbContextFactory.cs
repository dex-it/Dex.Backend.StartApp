using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StartApp.Infrastructure.Provider;

namespace StartApp.Ef.Migrator
{
    internal class DbContextFactory : IDesignTimeDbContextFactory<MigratorDbContext>
    {
        public MigratorDbContext CreateDbContext(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{Program.EnvironmentName}.json", true)
                .AddJsonFile("appsettings.local.json", true);

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
    }
}