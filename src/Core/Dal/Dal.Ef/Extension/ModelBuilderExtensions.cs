using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Dal.Ef.Extension
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder UseValueConverterForType<TModel, TProvider>(
            this ModelBuilder modelBuilder,
            ValueConverter<TModel, TProvider> converter)
        {
            return UseValueConverterForType(modelBuilder, typeof(TModel), converter);
        }

        internal static ModelBuilder UseDateTimeConverter(this ModelBuilder modelBuilder)
        {
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            UseValueConverterForType(modelBuilder, dateTimeConverter);
            
            var dateTimeNullableConverter = new ValueConverter<DateTime?, DateTime?>(
                v => v, v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : (DateTime?) null);
            UseValueConverterForType(modelBuilder, dateTimeNullableConverter);

            return modelBuilder;
        }

        private static ModelBuilder UseValueConverterForType(
            this ModelBuilder modelBuilder, 
            Type type,
            ValueConverter converter)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.ClrType
                    .GetProperties()
                    .Where(p => p.PropertyType == type);
                
                foreach (var property in properties)
                {
                    modelBuilder
                        .Entity(entityType.Name)
                        .Property(property.Name)
                        .HasConversion(converter);
                }
            }

            return modelBuilder;
        }
    }
}