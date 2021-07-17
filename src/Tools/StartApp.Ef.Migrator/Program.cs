using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Mono.Options;

namespace StartApp.Ef.Migrator
{
    public static class Program
    {
        public static string EnvironmentName { get; private set; } = "Development";

        private static void Main(string[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            var dataOptions = new OptionSet {{"environment=", s => EnvironmentName = s}};
            var actionOptions = new OptionSet {{"migrate", _ => Migrate()}};

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
            using var context = new DbContextFactory().CreateDbContext(Array.Empty<string>());
            context.Database.Migrate();
        }
    }
}