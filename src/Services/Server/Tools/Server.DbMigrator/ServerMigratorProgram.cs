using System;
using System.IO;
using System.Linq;
using Dex.Ef.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mono.Options;
using Server.Dal;
using Server.Dal.Provider;

namespace Server.DbMigrator
{
    public static class ServerMigratorProgram
    {
        public static string EnvironmentName = "Development";

        private static void Main(string[] args)
        {
            var dataOptions = new OptionSet
            {
                {"environment=", s => EnvironmentName = s}
            };
            var actionOptions = new OptionSet
            {
                {"migrate", _ => Migrate()}
            };

            if (args.Any() == false)
            {
                Console.WriteLine("Support parameters");
                dataOptions.WriteOptionDescriptions(Console.Out);
                actionOptions.WriteOptionDescriptions(Console.Out);
                Console.ReadKey();
            }

            dataOptions.Parse(args);
            actionOptions.Parse(args);
        }

        private static void Migrate()
        {
            var context = new DbContextFactory().CreateDbContext(Array.Empty<string>());
            context.Database.Migrate();
        }
    }

    internal class DbContextFactory : IDesignTimeDbContextFactory<DbContextFactory.MigratorEfDataConnection>
    {
        public MigratorEfDataConnection CreateDbContext(string[] args)
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
            
            return new MigratorEfDataConnection(modelStore, dbContextOptions, loggerFactory);
        }

        internal class MigratorEfDataConnection : ServerEfDataConnection
        {
            private readonly ILoggerFactory _loggerFactory;

            public MigratorEfDataConnection(
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
        }
    }
}