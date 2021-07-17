using System;
using System.Collections.Generic;
using Dex.Ef.Contracts;
using Microsoft.EntityFrameworkCore;

namespace StartApp.Infrastructure.Contexts
{
    public class ServerDbContext : DbContext
    {
        private readonly IEnumerable<Type> _modelTypes;
        
        public ServerDbContext(IModelStore modelStore, DbContextOptions options)
            : base(options)
        {
            if (modelStore == null) throw new ArgumentNullException(nameof(modelStore));
            _modelTypes = modelStore.GetModels();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (Type modelType in _modelTypes)
            {
                modelBuilder.Entity(modelType);
            }
            
            base.OnModelCreating(modelBuilder);
        }
    }
}