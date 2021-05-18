using Dal.Ef.Base;
using Dex.Ef.Contracts;
using Microsoft.EntityFrameworkCore;
using Server.Dal.EF.Provider;

namespace Server.Dal.EF
{
    internal class ServerDbContext : BaseDbContext
    {
        public ServerDbContext(IModelStore modelStore, DbContextOptions options)
            : base(modelStore, options)
        {
        }

        static ServerDbContext()
        {
            EnumFluentDbProvider.MapEnum();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasPostgresExtension("uuid-ossp");

            EnumFluentDbProvider.Config(builder);
            FluentIndex.Config(builder);
            ForeignKeysFluentDbProvider.Config(builder);
        }
    }
}