using System;
using System.Collections.Generic;
using System.Diagnostics;
using Dex.Ef.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Server.Dal.Base;
using Server.Dal.Extension;
using Server.Dal.Provider;

namespace Server.Dal
{
    internal class ServerEfDataConnection : ResetDbContext
    {
        private readonly IEnumerable<Type> _modeTypes;

        public ServerEfDataConnection(IModelStore modelStore, DbContextOptions options) : base(options)
        {
            _modeTypes = modelStore.GetModels();
        }

        static ServerEfDataConnection()
        {
            EnumFluentDbProvider.MapEnum();
        }

        internal override void Reset()
        {
            ChangeTracker.Clear();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var sw = Stopwatch.StartNew();
            foreach (var modeType in _modeTypes)
            {
                builder.Entity(modeType);
            }

            builder.HasPostgresExtension("uuid-ossp");
            Trace.WriteLine("OnModelCreating: " + sw.Elapsed);

            EnumFluentDbProvider.Config(builder);
            FluentIndex.Config(builder);
            ForeignKeysFluentDbProvider.Config(builder);
            DataSeedingProvider.Config(builder);
            
            DateConverter(builder);
        }

        private static void DateConverter(ModelBuilder builder)
        {
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            var dateTimeNullableConverter = new ValueConverter<DateTime?, DateTime?>(
                v => v, v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : (DateTime?) null);

            builder.UseValueConverterForType(dateTimeConverter);
            builder.UseValueConverterForType(dateTimeNullableConverter);
        }
    }
}