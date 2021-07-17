using Microsoft.EntityFrameworkCore;
using System;

namespace StartApp.Ef.Migrator
{
    internal static class DataSeedingProvider
    {
        public static void Config(ModelBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
        }
    }
}