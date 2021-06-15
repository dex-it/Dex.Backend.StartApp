using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Mono.Options;

namespace StartApp.Ef.Migrator
{
    public static class Program
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
}