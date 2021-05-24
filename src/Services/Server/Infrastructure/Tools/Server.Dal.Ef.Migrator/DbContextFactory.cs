using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Server.Dal.EF.Provider;

namespace Server.Dal.Ef.Migrator
{
    internal class DbContextFactory : IDesignTimeDbContextFactory<MigratorDbContext>
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
    }
}