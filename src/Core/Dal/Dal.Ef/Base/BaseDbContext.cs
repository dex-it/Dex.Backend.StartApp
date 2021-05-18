using System;
using System.Collections.Generic;
using Dal.Ef.Extension;
using Dex.Ef.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Dal.Ef.Base
{
    public abstract class BaseDbContext : ResetDbContext
    {
        private IEnumerable<Type> ModeTypes { get; }

        protected BaseDbContext(IModelStore modelStore)
        {
            ModeTypes = modelStore.GetModels();
        }

        protected BaseDbContext(IModelStore modelStore, DbContextOptions options) 
            : base(options)
        {
            ModeTypes = modelStore.GetModels();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var modeType in ModeTypes)
            {
                builder.Entity(modeType);
            }

            builder.UseDateTimeConverter();
        }
    }
}