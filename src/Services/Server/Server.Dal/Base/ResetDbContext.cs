using Microsoft.EntityFrameworkCore;

namespace Server.Dal.Base
{
    internal abstract class ResetDbContext : DbContext
    {
        protected ResetDbContext(DbContextOptions options) : base(options)
        {
        }

        internal abstract void Reset();
    }
}